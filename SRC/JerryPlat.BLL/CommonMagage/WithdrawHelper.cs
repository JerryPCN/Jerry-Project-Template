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
    public class WithdrawHelper : BaseHelper
    {
        public PageData<WithdrawDto> GetPageList(SearchModel searchModel, Expression<Func<Withdraw, DateTime>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true, bool bIsMobile = false)
        {
            IQueryable<Withdraw> withdrawQuerable = GetDbSet<Withdraw>();

            if (searchModel.Id != 0)
            {
                withdrawQuerable = withdrawQuerable.Where(o => o.WithdrawTypeId == searchModel.Id);
            }

            if (searchModel.Id1 != 0)
            {
                withdrawQuerable = withdrawQuerable.Where(o => o.Status == (WithdrawStatus)searchModel.Id1);
            }

            if (!string.IsNullOrEmpty(searchModel.SearchText))
            {
                withdrawQuerable = withdrawQuerable.Where(o => o.Name.Contains(searchModel.SearchText)
                | o.Phone.Contains(searchModel.SearchText)
                | o.Description.Contains(searchModel.SearchText));
            }

            if (searchModel.StartTime.HasValue)
            {
                withdrawQuerable = withdrawQuerable.Where(o => o.ApplyTime >= searchModel.StartTime.Value);
            }

            if (searchModel.EndTime.HasValue)
            {
                withdrawQuerable = withdrawQuerable.Where(o => o.ApplyTime < searchModel.EndTime.Value);
            }

            if (bIsMobile)
            {
                Member member = SessionHelper.Mob.GetSession<Member>();
                withdrawQuerable = withdrawQuerable.Where(o => o.MemberId == member.Id);
            }

            PageData<Withdraw> withdrawList = PageHelper.GetPageData(withdrawQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);

            PageData<WithdrawDto> withdrawDtoList = new PageData<WithdrawDto>(withdrawList.PageParam, withdrawList.PageModel.TotalItem);

            List<int> memberIdList = withdrawList.Data.Select(o => o.MemberId).ToList();
            List<Member> memberList = GetDbSet<Member>().Where(o => memberIdList.Contains(o.Id)).ToList();

            List<int> withdrawTypeIdList = withdrawList.Data.Select(o => o.WithdrawTypeId).ToList();
            List<WithdrawType> withdrawTypeList = GetDbSet<WithdrawType>().Where(o => withdrawTypeIdList.Contains(o.Id)).ToList();

            withdrawDtoList.Data = withdrawList.Data.Select(o => new WithdrawDto
            {
                Withdraw = o,
                Member = memberList.Where(a => a.Id == o.MemberId).FirstOrDefault(),
                WithdrawType = withdrawTypeList.Where(a => a.Id == o.WithdrawTypeId).Select(a => a.Name).FirstOrDefault(),
                WithdrawStatus = o.Status.GetDescription()
            }).ToList();

            return withdrawDtoList;
        }
        
        public WithdrawScoreDto GetWithdrawScore(bool bIsMobile = false)
        {
            IQueryable<Withdraw> withdrawQuerable = GetDbSet<Withdraw>();
            Member member = null;
            if (bIsMobile)
            {
                member = SessionHelper.Mob.GetSession<Member>();
                MemberHelper memberHelper = new MemberHelper();
                Member memberDb = memberHelper.GetById(member.Id);
                if (memberDb.Score != member.Score)
                {
                    member = memberDb;
                    SessionHelper.Mob.SetSession(member);
                }
                withdrawQuerable = withdrawQuerable.Where(o => o.MemberId == member.Id);
            }
            List<Withdraw> withdrawList = withdrawQuerable.ToList();
            return new WithdrawScoreDto
            {
                Score = member == null ? 0 : member.Score,
                TotalWithdrawApplyScore = withdrawList.Select(o => o.Score).DefaultIfEmpty().Sum(),
                TotalWithdrawDiscardScore = withdrawList.Where(o => o.Status == WithdrawStatus.DISCARDED).Select(o => o.Score).DefaultIfEmpty().Sum(),
                TotalWithdrawScore = withdrawList.Where(o => o.Status == WithdrawStatus.APPROVED).Select(o => o.Score).DefaultIfEmpty().Sum(),
                TotalTaxScore = withdrawList.Where(o => o.Status == WithdrawStatus.APPROVED).Select(o => o.Score * o.TaxPercentage).DefaultIfEmpty().Sum()
            };
        }

        public async Task<bool> SaveAsync(Withdraw model)
        {
            return await SaveAsync<Withdraw>(model);
        }

        public async Task<bool> Discard(int id)
        {
            Withdraw withdraw = await GetByIdAsync<Withdraw>(id);
            if (withdraw.Status != WithdrawStatus.APPLY)
            {
                return false;
            }
            withdraw.Status = WithdrawStatus.DISCARDED;
            withdraw.OperateTime = DateTime.Now;
            return await SaveChangesAsync();
        }

        public async Task<bool> Approve(int id)
        {
            Withdraw withdraw = await GetByIdAsync<Withdraw>(id);
            if (withdraw.Status == WithdrawStatus.APPROVED)
            {
                return true;
            }

            Member member = await GetByIdAsync<Member>(withdraw.MemberId);
            if (member.Score < withdraw.Score)
            {
                return false;
            }

            withdraw.Status = WithdrawStatus.APPROVED;
            withdraw.OperateTime = DateTime.Now;

            WithdrawType withdrawType = await GetDbSet<WithdrawType>().Where(o => o.Id == withdraw.WithdrawTypeId).FirstOrDefaultAsync();

            PayRecordHelper payRecordHelper = new PayRecordHelper();
            await payRecordHelper.SaveAsycn(withdraw, withdrawType, member);

            string strMsg = string.Empty;
            SMSHelper.SendPay(member.Phone, withdraw.Score - withdraw.Score * withdraw.TaxPercentage, withdrawType.Name, out strMsg);

            MemberHelper memberHelper = new MemberHelper();
            await memberHelper.AddScore(member, -withdraw.Score, withdrawType.Name + "提现", true);

            return await SaveChangesAsync();
        }
    }
}