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
    public class PayRecordHelper : BaseHelper
    {
        public PageData<PayRecordDto> GetPageList(SearchModel searchModel, Expression<Func<PayRecord, DateTime>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
        {
            IQueryable<PayRecord> payRecordQuerable = GetDbSet<PayRecord>();

            if (searchModel.Id != 0)
            {
                payRecordQuerable = payRecordQuerable.Where(o => o.MemberId == searchModel.Id);
            }

            PageData<PayRecord> payRecordList = PageHelper.GetPageData(payRecordQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);

            PageData<PayRecordDto> payRecordDtoList = new PageData<PayRecordDto>(payRecordList.PageParam, payRecordList.PageModel.TotalItem);

            List<int> memberIdList = payRecordList.Data.Select(o => o.MemberId).ToList();
            List<Member> memberList = GetDbSet<Member>().Where(o => memberIdList.Contains(o.Id)).ToList();

            payRecordDtoList.Data = payRecordList.Data.Select(o => new PayRecordDto
            {
                PayRecord = o,
                Name = memberList.Where(a => a.Id == o.MemberId).Select(a => a.Name).FirstOrDefault()
            }).ToList();

            return payRecordDtoList;
        }

        public async Task<bool> SaveAsycn(Enroll model, bool bIsPaid)
        {
            PayRecord payRecord = new PayRecord
            {
                MemberId = model.MemberId,
                OrderNo = model.OrderNo,
                Description = (bIsPaid ? "" : "撤消") + "考试费用",
                Amount = bIsPaid ? model.Amount : -model.Amount,
                PayType = (bIsPaid ? "" : "撤消") + model.PayType
            };

            return await SaveAsync(payRecord);
        }

        public async Task<bool> SaveAsycn(Withdraw model, WithdrawType withdrawType, Member member)
        {
            PayRecord payRecord = new PayRecord
            {
                MemberId = model.MemberId,
                OrderNo = "Withdraw:" + model.Id,
                Description = $"用户{member.Name}于{model.OperateTime.ToFormat()}提现",
                Amount = -(model.Score - model.Score * model.TaxPercentage),
                PayType = withdrawType?.Name + model.Description
            };

            return await SaveAsync(payRecord);
        }
    }
}