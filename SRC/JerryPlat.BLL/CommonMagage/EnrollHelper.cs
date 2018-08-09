using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JerryPlat.BLL.CommonMagage
{
    public class EnrollHelper : BaseHelper
    {
        public Course GetCourse(int id)
        {
            return _Db.Courses.First(o => o.Id == id);
        }

        public async Task<Enroll> GetEnroll(int id, bool bIsNotNull = true)
        {
            return await GetByIdAsync<Enroll>(id, null, bIsNotNull);
        }

        public Enroll GetEnroll(string strOrderNo = "")
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            return GetEnroll(member, strOrderNo);
        }

        public Enroll GetEnroll(Member member, string strOrderNo = "")
        {
            if (SessionHelper.Mob.IsNullSession())
            {
                return null;
            }

            IQueryable<Enroll> enrollQueryable = _Db.Enrolls.Where(o => o.MemberId == member.Id);
            if (!string.IsNullOrEmpty(strOrderNo))
            {
                enrollQueryable = enrollQueryable.Where(o => o.OrderNo == strOrderNo);
            }
            return enrollQueryable.FirstOrDefault();
        }

        public EnrollDto GetEnrollDto(string strOrderNo = "")
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            return GetEnrollDto(member, strOrderNo);
        }

        public EnrollDto GetEnrollDto(Member member, string strOrderNo = "")
        {
            if (SessionHelper.Mob.IsNullSession())
            {
                return null;
            }

            IQueryable<Enroll> enrollQueryable = _Db.Enrolls.Where(o => o.MemberId == member.Id);
            if (!string.IsNullOrEmpty(strOrderNo))
            {
                enrollQueryable = enrollQueryable.Where(o => o.OrderNo == strOrderNo);
            }
            return (from a in enrollQueryable
                    join b in _Db.Courses on a.CourseId equals b.Id
                    select new EnrollDto
                    {
                        Enroll = a,
                        Course = b.Name
                    }).FirstOrDefault();
        }

        public bool IsEnrolled()
        {
            return IsEnrolled(GetEnroll());
        }

        public bool IsEnrolled(Enroll enroll)
        {
            return enroll != null;
        }

        public bool IsPaid()
        {
            Enroll enroll = GetEnroll();
            return IsPaid(enroll);
        }

        public bool IsExistOrderNo(string strOrderNo)
        {
            return _Db.Enrolls.Where(o => o.OrderNo == strOrderNo).Any();
        }

        public async Task<bool> IsAutoScore(Enroll enroll)
        {
            MemberHelper memberHelper = new MemberHelper();
            return await memberHelper.IsAutoScored(enroll.PaidTime.Value);
        }

        public async Task Approve(Enroll enroll)
        {
            enroll.Status = EnrollStatus.PaySuccess;
            await SetPayStatus(enroll);
        }

        public async Task Reverse(Enroll enroll)
        {
            enroll.Status = EnrollStatus.ReversePaid;
            await SetPayStatus(enroll);
        }

        public async Task SetPayStatus(EnrollStatus enrollStatus, PayModelDto model)
        {
            await SetPayStatus(enrollStatus, model.OrderNo, model.PayType);
        }

        public async Task SetPayStatus(Enroll model)
        {
            Enroll enroll = await GetEnroll(model.Id, false);
            if (enroll == null || enroll.Status == model.Status)
            {
                return;
            }
            MemberHelper memberHelper = new MemberHelper();
            Member member = await memberHelper.GetByIdAsync(model.MemberId);
            await SetPayStatus(enroll, member, model.Status, model.PayType);
        }

        public async Task SetPayStatus(EnrollStatus enrollStatus, string strOrderNo, string strPayType)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            Enroll enroll = GetEnroll(member, strOrderNo);
            if (enroll == null || enroll.Status == enrollStatus)
            {
                return;
            }
            await SetPayStatus(enroll, member, enrollStatus, strPayType);
        }

        public async Task SetPayStatus(Enroll enroll, Member member, EnrollStatus enrollStatus, string strPayType)
        {
            if (enroll.Status == enrollStatus)
            {
                return;
            }

            enroll.Status = enrollStatus;
            enroll.PaidTime = DateTime.Now;

            bool bIsPaid = enrollStatus == EnrollStatus.PaySuccess;

            if (bIsPaid)//支付成功
            {
                enroll.PayType = strPayType;
                enroll.Note = "已完成支付";
            }
            else
            {
                enroll.PayType = "";
                enroll.Note = "已完成撤销支付";
            }

            PayRecordHelper helper = new PayRecordHelper();
            await helper.SaveAsycn(enroll, bIsPaid);
            await ScoreRule(enroll, member, bIsPaid, true);

            await SaveChangesAsync();
        }

        //A---->B---->C
        //1，C报名支付后，B获取500， A获取200
        public async Task ScoreRule(Enroll enroll, Member member, bool bIsPaid, bool bIsSaveToDb = true)//C
        {
            if (member == null)
            {
                return;
            }

            MemberHelper memberHelper = new MemberHelper();

            Member referee = null;
            if (member.RefereeId > 0)
            {
                referee = await memberHelper.GetByIdAsync(member.RefereeId, false);//B
            }
            if (referee == null)
            {
                return;
            }

            if (enroll.RefereeId != referee.Id)
            {
                return;
            }

            //B获取500
            await memberHelper.AddScore(referee, SystemConfigModel.Instance.RefereeScore, SystemConfigModel.Instance.RefereeScoreDescription, bIsPaid, bIsSaveToDb);

            Member parent = null;
            if (referee.RefereeId > 0)
            {
                parent = await memberHelper.GetByIdAsync(referee.RefereeId);//A
            }

            if (parent == null)
            {
                return;
            }
            //A获取200
            await memberHelper.AddScore(parent, SystemConfigModel.Instance.ParentRefereeScore, SystemConfigModel.Instance.ParentRefereeScoreDescription, bIsPaid, bIsSaveToDb);
        }

        public bool IsPaid(Enroll enroll)
        {
            if (enroll == null)
            {
                return false;
            }
            return enroll.Status == EnrollStatus.PaySuccess;
        }

        public async Task<bool> Save(Enroll model)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            return await Save(model, member);
        }

        public async Task<bool> Save(Enroll model, Member member)
        {
            model.MemberId = member.Id;
            model.Status = EnrollStatus.Apply;
            member = _Db.Members.Where(o => o.Id == member.Id).FirstOrDefault();
            if (member == null)
            {
                throw new Exception("Not exist Member with Id=" + model.MemberId);
            }
            member.Phone = model.Phone;
            member.Name = model.Name;
            member.RefereeId = model.RefereeId;

            SessionHelper.Mob.SetSession(member);
            return await SaveAsync(model);
        }

        public IQueryable<Enroll> GetQueryableEnrollList(SearchModel searchModel)
        {
            IQueryable<Enroll> enrollQuerable = GetDbSet<Enroll>();
            if (searchModel.Id != 0)
            {
                enrollQuerable = enrollQuerable.Where(o => o.CourseId == searchModel.Id);
            }

            if (searchModel.Id1 != 0)
            {
                enrollQuerable = enrollQuerable.Where(o => o.Status == (EnrollStatus)searchModel.Id1);
            }

            if (searchModel.StartTime.HasValue)
            {
                enrollQuerable = enrollQuerable.Where(o => o.UpdateTime >= searchModel.StartTime.Value);
            }

            if (searchModel.EndTime.HasValue)
            {
                enrollQuerable = enrollQuerable.Where(o => o.UpdateTime < searchModel.EndTime.Value);
            }

            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
                enrollQuerable = enrollQuerable.Where(o => o.PayType.Contains(searchModel.SearchText));
            }

            return enrollQuerable;
        }

        public PageData<EnrollDto> GetPageList(SearchModel searchModel, Expression<Func<Enroll, DateTime>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
        {
            IQueryable<Enroll> enrollQuerable = GetQueryableEnrollList(searchModel);

            PageData<Enroll> enrollList = PageHelper.GetPageData(enrollQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);

            PageData<EnrollDto> enrollDtoList = new PageData<EnrollDto>(enrollList.PageParam, enrollList.PageModel.TotalItem);

            List<int> courseIdList = enrollList.Data.Select(o => o.CourseId).ToList();
            List<Course> courseList = GetDbSet<Course>().Where(o => courseIdList.Contains(o.Id)).ToList();

            enrollDtoList.Data = enrollList.Data.Select(o => new EnrollDto
            {
                Enroll = o,
                Course = courseList.Where(a => a.Id == o.CourseId).Select(a => a.Name).FirstOrDefault()
            }).ToList();

            return enrollDtoList;
        }

        public List<EnrollExportDto> GetPageList(SearchModel searchModel, Expression<Func<EnrollExportDto, DateTime>> orderByKeySelector, bool bIsAscOrder = true)
        {
            IQueryable<Enroll> enrollQuerable = GetQueryableEnrollList(searchModel);

            IQueryable<EnrollExportDto> enrollExportDtoQuerable = (from a in enrollQuerable
                                                                   join b in _Db.Courses on a.CourseId equals b.Id
                                                                   join c in _Db.Members on a.RefereeId equals c.Id into dd
                                                                   from d in dd.DefaultIfEmpty()
                                                                   select new EnrollExportDto
                                                                   {
                                                                       Id = a.Id,
                                                                       ShareCode = a.ShareCode,
                                                                       OrderNo = a.OrderNo,
                                                                       MemberId = a.MemberId,
                                                                       Name = a.Name,
                                                                       RefereeId = a.RefereeId,
                                                                       RefereeName = d.Name,
                                                                       Phone = a.Phone,
                                                                       CourseId = a.CourseId,
                                                                       Course = b.Name,
                                                                       City = a.City,
                                                                       Ground = a.Ground,
                                                                       School = a.School,
                                                                       IdCard = a.IdCard,
                                                                       Address = a.Address,
                                                                       Amount = a.Amount,
                                                                       Status = a.Status,
                                                                       PaidTime = a.PaidTime,
                                                                       PayType = a.PayType,
                                                                       Note = a.Note,
                                                                       UpdateTime = a.UpdateTime
                                                                   });

            return PageHelper.GetData(enrollExportDtoQuerable, orderByKeySelector, bIsAscOrder, searchModel.Sort);
        }
    }
}