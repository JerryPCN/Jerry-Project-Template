using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JerryPlat.BLL.CommonMagage
{
    public class MemberHelper : BaseHelper
    {
        public IQueryable<Member> GetQueryableMemberList(SearchModel searchModel)
        {
            IQueryable<Member> memberQuerable = GetDbSet<Member>();

            if (searchModel.Id != 0)
            {
                memberQuerable = memberQuerable.Where(o => o.RefereeId == searchModel.Id);
            }

            if (searchModel.StartTime.HasValue)
            {
                memberQuerable = memberQuerable.Where(o => o.JoinTime >= searchModel.StartTime.Value);
            }

            if (searchModel.EndTime.HasValue)
            {
                memberQuerable = memberQuerable.Where(o => o.JoinTime < searchModel.EndTime.Value);
            }

            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
                memberQuerable = memberQuerable.Where(o => o.Name.Contains(searchModel.SearchText) | o.NickName.Contains(searchModel.SearchText));
            }

            return memberQuerable;
        }

        public PageData<Member> GetTeamList(SearchModel searchModel, Expression<Func<Member, DateTime>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
        {
            IQueryable<Member> memberQuerable = GetQueryableMemberList(searchModel);
            return PageHelper.GetPageData(memberQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);
        }

        public List<Member> GetTeamList(SearchModel searchModel, Expression<Func<Member, DateTime>> orderByKeySelector, bool bIsAscOrder = true)
        {
            IQueryable<Member> memberQuerable = GetQueryableMemberList(searchModel);
            return PageHelper.GetData(memberQuerable, orderByKeySelector, bIsAscOrder, searchModel.Sort);
        }

        public PageData<TeamDto> GetTeamReportList(SearchModel searchModel, Expression<Func<Member, DateTime>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
        {
            IQueryable<Member> memberQuerable = GetQueryableMemberList(searchModel);
            PageData<Member> teamList = PageHelper.GetPageData(memberQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);
            PageData<TeamDto> teamDtoList = new PageData<TeamDto>(teamList.PageParam, teamList.PageModel.TotalItem);

            List<int> refereeIdList = teamList.Data.Select(o => o.Id).ToList();

            teamDtoList.Data = (
                                from a in memberQuerable.Where(o => refereeIdList.Contains(o.Id))//二级学员
                                let note = _Db.Enrolls.Where(o => o.MemberId == a.Id).Select(o => o.Note).FirstOrDefault()

                                let thirdMemberList = _Db.Members.Where(o => o.RefereeId == a.Id).Select(o => o.Id) //三级学员
                                let thirdEnrollList = _Db.Enrolls.Where(o => thirdMemberList.Contains(o.MemberId))
                                let totalMember = thirdEnrollList.Count()
                                let paidMember = thirdEnrollList.Where(o => o.Status == EnrollStatus.PaySuccess).Count()

                                let totalScore = _Db.ScoreHistories.Where(o => o.MemberId == a.Id & o.Different > 0).Select(o => o.Different).DefaultIfEmpty().Sum()
                                let withdrawScore = _Db.ScoreHistories.Where(o => o.MemberId == a.Id & o.Different < 0).Select(o => o.Different).DefaultIfEmpty().Sum()
                                select new TeamDto
                                {
                                    Member = a,
                                    Note = note,
                                    TotalMember = totalMember,
                                    PaidMember = paidMember,
                                    TotalScore = totalScore,
                                    WithdrawScore = withdrawScore
                                }).ToList();

            teamDtoList.Data.ForEach(o =>
            {
                if (!string.IsNullOrEmpty(o.Member.Name))
                {
                    o.Member.Name = o.Member.Name.ToSpecialString(1, o.Member.Name.Length);
                }

                if (!string.IsNullOrEmpty(o.Member.Phone))
                {
                    o.Member.Phone = o.Member.Phone.ToSpecialString(3, 4);
                }
            });

            return teamDtoList;
        }

        public PageData<ScoreHistory> GetScoreList(SearchModel searchModel, Expression<Func<ScoreHistory, DateTime>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
        {
            IQueryable<ScoreHistory> scoreHistoryQuerable = GetDbSet<ScoreHistory>();

            if (searchModel.Id != 0)
            {
                scoreHistoryQuerable = scoreHistoryQuerable.Where(o => o.MemberId == searchModel.Id);
            }

            return PageHelper.GetPageData(scoreHistoryQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);
        }

        public List<ScoreHistory> GetScoreList(SearchModel searchModel, Expression<Func<ScoreHistory, DateTime>> orderByKeySelector, bool bIsAscOrder = true)
        {
            IQueryable<ScoreHistory> scoreHistoryQuerable = GetDbSet<ScoreHistory>();

            if (searchModel.Id != 0)
            {
                scoreHistoryQuerable = scoreHistoryQuerable.Where(o => o.MemberId == searchModel.Id);
            }
            return PageHelper.GetData(scoreHistoryQuerable, orderByKeySelector, bIsAscOrder, searchModel.Sort);
        }

        public async Task<bool> AddScore(int intScore, string strDescription, bool bIsAdd, bool bIsSaveToDb = true)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            return await AddScore(member, intScore, strDescription, bIsAdd, bIsSaveToDb);
        }

        public async Task<bool> AddScore(Member member, int intScore, string strDescription, bool bIsAdd, bool bIsSaveToDb = true)
        {
            Attach<Member>(member);
            ScoreHistory scoreHistory = new ScoreHistory
            {
                MemberId = member.Id,
                Different = (bIsAdd ? intScore : -intScore),
                Description = (bIsAdd ? "" : "撤消") + strDescription
            };
            _Db.ScoreHistories.Add(scoreHistory);
            member.Score = member.Score + (bIsAdd ? intScore : -intScore);
            if (bIsSaveToDb)
            {
                return await SaveChangesAsync();
            }
            return true;
        }

        public async Task<Member> GetByShareCodeAsync(string strShareCode)
        {
            return await _Db.Members.Where(o => o.ShareCode == strShareCode).FirstOrDefaultAsync();
        }

        public Member GetById(int intId, bool bIsNotNull = true)
        {
            return GetById<Member>(intId, null, bIsNotNull);
        }

        public async Task<Member> GetByIdAsync(int intId, bool bIsNotNull = true)
        {
            return await GetByIdAsync<Member>(intId, null, bIsNotNull);
        }

        public async Task SetMemberReferee(string strShareCode)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            await SetMemberReferee(member, strShareCode);
        }

        public async Task SetMemberReferee(Member member, string strShareCode)
        {
            //LogHelper.Info($"SetMemberReferee:RefereeId:{member.RefereeId},Code:{strShareCode}");

            if (member.RefereeId > 0)
            {
                if (member.Id == member.RefereeId)
                {
                    member.RefereeId = 0;
                    await SaveAsync(member);
                    SessionHelper.Mob.SetSession(member);
                }

                return;
            }

            Member referee = await GetByShareCodeAsync(strShareCode);

            if (referee == null || member.Id == referee.Id)
            {
                return;
            }

            member.RefereeId = referee.Id;

            SessionHelper.Mob.SetSession(member);

            await SaveAsync(member);
        }

        public async Task SetLocation(LocationDto location)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            member.Latitude = location.Latitude;
            member.Longitude = location.Longitude;
            member.Precision = location.Accuracy;

            SessionHelper.Mob.SetSession(member);

            await SaveAsync(member);
        }

        public async Task<bool> Bind(BindDto model)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            member.Phone = model.Phone;
            SessionHelper.Mob.SetSession(member);
            return await SaveAsync(member);
        }

        private IQueryable<Enroll> GetEnrollPaidQueryable(DateTime startTime, DateTime endTime)
        {
            return _Db.Enrolls.Where(o => o.Status == EnrollStatus.PaySuccess
                                & o.PaidTime >= startTime & o.PaidTime < endTime);
        }

        private IQueryable<Member> GetEnrollPaidMemberQueryable(DateTime startTime, DateTime endTime)
        {
            return
                from a in GetEnrollPaidQueryable(startTime, endTime)
                join b in _Db.Members.Where(o => o.RefereeId > 0) on a.MemberId equals b.Id
                select b;
        }

        private List<MemberScoreDto> GetMemberScoreDtoList(List<Member> refereeMemberList, List<Member> enrollPaidMemberList, List<MemberTimeDto> memberTimeDtoList)
        {
            List<MemberScoreDto> list = new List<MemberScoreDto>();
            MemberScoreDto dto = null;
            List<int> firstList = new List<int>(); ;
            List<int> secondList = new List<int>(); ;
            foreach (Member member in refereeMemberList)
            {
                if (list.Any(o => o.Member.Id == member.Id))
                {
                    continue;
                }

                dto = new MemberScoreDto { Member = member };

                firstList.Clear();
                firstList.AddRange(refereeMemberList.Where(o => o.RefereeId == member.Id).Select(o => o.Id));
                firstList.AddRange(enrollPaidMemberList.Where(o => o.RefereeId == member.Id).Select(o => o.Id));
                firstList = firstList.Distinct().ToList();
                dto.FirstCount = firstList.Count;

                dto.LastTime = memberTimeDtoList.Where(o => o.MemberId == member.Id).Select(o => o.LastTime).FirstOrDefault();

                secondList.Clear();
                secondList.AddRange(refereeMemberList.Where(o => firstList.Contains(o.RefereeId)).Select(o => o.Id));
                secondList.AddRange(enrollPaidMemberList.Where(o => firstList.Contains(o.RefereeId)).Select(o => o.Id));
                secondList = secondList.Distinct().ToList();
                dto.FirstCount = firstList.Count;

                list.Add(dto);
            }
            return list;
        }

        public async Task<bool> AutoScoreMonthly(DateTime startTime, DateTime endTime)
        {
            if (!(await AutoScore(startTime, endTime, false)))
            {
                return false;
            }
            return await AddMemberScoreHistory(startTime);
        }

        private async Task<bool> AddMemberScoreHistory(DateTime startTime)
        {
            AdminUser adminUser = SessionHelper.Admin.GetSession<AdminUser>();

            return await SaveAsync(new MemberScoreHistory
            {
                MonthTime = startTime,
                AdminUserId = adminUser.Id,
                OperateTime = DateTime.Now
            });
        }

        public async Task<bool> IsAutoScored(DateTime startTime)
        {
            return await _Db.MemberScoreHistories.AnyAsync(o => o.MonthTime == startTime);
        }

        //A--->B--->C
        public async Task<bool> AutoScore(DateTime startTime, DateTime endTime, bool bIsSaveToDb = true)
        {
            List<MemberTimeDto> memberTimeDtoList = (from a in GetEnrollPaidQueryable(startTime, endTime)
                                                     group a by a.MemberId into b
                                                     select new MemberTimeDto
                                                     {
                                                         MemberId = b.Key,
                                                         LastTime = b.Max(o => o.PaidTime)
                                                     }).ToList();

            IQueryable<Member> enrollPaidMemberQueryable = GetEnrollPaidMemberQueryable(startTime, endTime);

            //当月已支付的推荐学员
            List<Member> enrollPaidMemberList = enrollPaidMemberQueryable.Distinct().ToList();

            //当月存在已支付被推荐的学员的推荐人
            List<Member> refereeMemberList = ((from a in enrollPaidMemberQueryable
                                               join b in _Db.Members on a.RefereeId equals b.Id
                                               select b).Union(
                                                from a in enrollPaidMemberQueryable
                                                join b in _Db.Members.Where(o => o.RefereeId > 0) on a.RefereeId equals b.Id
                                                join c in _Db.Members on b.RefereeId equals c.Id
                                                select c
                                            )).Distinct().ToList();

            List<MemberScoreDto> memberScoreDtoList = GetMemberScoreDtoList(refereeMemberList, enrollPaidMemberList, memberTimeDtoList);
            if (!memberScoreDtoList.Any())
            {
                return true;
            }

            /*
             * 2，(B1,B2…) >=10人，A获取每人100，包括前10人
             *    (C1,C2…) >=30人，A获取每人50，包括前30人
             */
            memberScoreDtoList.Where(o => o.FirstCount >= SystemConfigModel.Instance.FirstCount).ToList().ForEach(async dto =>
            {
                await AddScore(dto.Member, dto.FirstCount * SystemConfigModel.Instance.FirstScore, SystemConfigModel.Instance.FirstDescription + ":" + dto.FirstCount + "人", true, false);
            });

            memberScoreDtoList.Where(o => o.SecondCount >= SystemConfigModel.Instance.SecondCount).ToList().ForEach(async dto =>
            {
                await AddScore(dto.Member, dto.SecondCount * SystemConfigModel.Instance.SecondScore, SystemConfigModel.Instance.SecondDescription + ":" + dto.SecondCount + "人", true, false);
            });

            /*
             *3，(B1,B2…) >=10人，A才有资格参与比赛，参与比赛第一名可拿下所有参与比赛B的人数*50
             */
            string[] aryGradePercentage = SystemConfigModel.Instance.GradePercentage.Split('+');

            List<MemberScoreDto> matchGradeList = memberScoreDtoList.Where(o => o.FirstCount >= SystemConfigModel.Instance.MatchCount)
                .OrderByDescending(o => o.FirstCount).ThenBy(o => o.LastTime).Take(aryGradePercentage.Length).ToList();

            int intTotalCount = enrollPaidMemberList.Count();
            int intTotalMathScore = intTotalCount * SystemConfigModel.Instance.MatchScore;

            for (int index = 0; index < matchGradeList.Count; index++)
            {
                await AddScore(matchGradeList[index].Member,
                    (int)Math.Floor(1m.ToMultiply(aryGradePercentage[index]) * intTotalMathScore),
                    "第" + (index + 1) + "名" + SystemConfigModel.Instance.MatchDescription + ", 分配比例为" + (100m.ToMultiply(aryGradePercentage[index])) + "%", true, false);
            }

            if (bIsSaveToDb)
            {
                return await SaveChangesAsync();
            }

            return true;
        }

        public async Task Import(List<Member> memberList)
        {
            List<MemberImportDto> importList = memberList.OrderBy(o => o.RefereeId)
                .Select(o => new MemberImportDto
                {
                    OldId = o.Id,
                    Member = o
                }).ToList();

            foreach (MemberImportDto import in importList)
            {
                import.Member.Id = 0;
                import.Member.Password = SystemConfigModel.Instance.DefaultPassword;
                _Db.Members.Add(import.Member);
            }
            await SaveChangesAsync();

            importList.Where(o => o.Member.RefereeId > 0).ToList().ForEach(
                o =>
                {
                    o.Member.RefereeId = (importList.Where(m => m.OldId == o.Member.RefereeId).Select(m => m.Member).FirstOrDefault()?.Id) ?? 0;
                });

            await SaveChangesAsync();
        }
    }
}