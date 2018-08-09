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
    public class SubscribeHelper : BaseHelper
    {
        public PageData<SubscribeDto> GetPageList(SearchModel searchModel, Expression<Func<Subscribe, DateTime>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true, bool bIsMobile = false)
        {
            IQueryable<Subscribe> enrollQuerable = GetDbSet<Subscribe>();

            if (searchModel.Id != 0)
            {
                enrollQuerable = enrollQuerable.Where(o => o.GroundId == searchModel.Id);
            }

            if (searchModel.Id1 != 0)
            {
                enrollQuerable = enrollQuerable.Where(o => o.CoachId == searchModel.Id1);
            }

            if (bIsMobile)
            {
                Member member = SessionHelper.Mob.GetSession<Member>();
                enrollQuerable = enrollQuerable.Where(o => o.MemberId == member.Id);
            }

            PageData<Subscribe> subscribeList = PageHelper.GetPageData(enrollQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);

            PageData<SubscribeDto> subscribeDtoList = new PageData<SubscribeDto>(subscribeList.PageParam, subscribeList.PageModel.TotalItem);

            List<int> memberIdList = subscribeList.Data.Select(o => o.MemberId).ToList();
            List<Member> memberList = GetDbSet<Member>().Where(o => memberIdList.Contains(o.Id)).ToList();

            List<int> courseIdList = subscribeList.Data.Select(o => o.CourseId).ToList();
            List<Course> courseList = GetDbSet<Course>().Where(o => courseIdList.Contains(o.Id)).ToList();

            List<int> groundIdList = subscribeList.Data.Select(o => o.GroundId).ToList();
            List<Ground> groundList = GetDbSet<Ground>().Where(o => groundIdList.Contains(o.Id)).ToList();

            List<int> coachIdList = subscribeList.Data.Select(o => o.CoachId).ToList();
            List<Coach> coachList = GetDbSet<Coach>().Where(o => coachIdList.Contains(o.Id)).ToList();

            subscribeDtoList.Data = subscribeList.Data.Select(o => new SubscribeDto
            {
                Subscribe = o,
                Name = memberList.Where(a => a.Id == o.MemberId).Select(a => a.Name).FirstOrDefault(),
                Course = courseList.Where(a => a.Id == o.CourseId).Select(a => a.Name).FirstOrDefault(),
                Ground = groundList.Where(a => a.Id == o.GroundId).Select(a => a.Name).FirstOrDefault(),
                Coach = coachList.Where(a => a.Id == o.CoachId).Select(a => a.Name).FirstOrDefault()
            }).ToList();

            return subscribeDtoList;
        }

        public async Task<bool> SaveAsync(Subscribe model)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            model.MemberId = member.Id;
            return await base.SaveAsync(model);
        }
    }
}