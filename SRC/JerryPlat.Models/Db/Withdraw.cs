using JerryPlat.Utils.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public enum WithdrawStatus
    {
        [Description("申请中")]
        APPLY = 1,
        [Description("已提现")]
        APPROVED = 2,
        [Description("已废弃")]
        DISCARDED = 3
    }

    public class Withdraw : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int MemberId { get; set; }
        public int WithdrawTypeId { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Account { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Phone { get; set; }

        public int Score { get; set; }
        public decimal TaxPercentage { get; set; }
        public DateTime ApplyTime { get; set; }
        public WithdrawStatus Status { get; set; }
        public DateTime OperateTime { get; set; }

        public Withdraw()
        {
            Status = WithdrawStatus.APPLY;
            ApplyTime = DateTime.Now;
            OperateTime = ApplyTime;
        }
    }
}