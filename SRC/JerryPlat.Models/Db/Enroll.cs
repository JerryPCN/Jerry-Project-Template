using JerryPlat.Utils.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public enum EnrollStatus
    {
        [Description("用户提交订单，尚未支付")]
        Apply = 1,

        [Description("用户支付成功")]
        PaySuccess = 2,

        [Description("用户支付失败")]
        PayFaild = 3,

        [Description("用户已支付申请")]
        ApplyPaid = 4,

        [Description("支付申请未通过")]
        ApplyPaidFaild = 5,
        
        [Description("支付已撤销")]
        ReversePaid = 6
    }

    public class Enroll : IEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string ShareCode { get; set; }

        [StringLength(200)]
        public string OrderNo { get; set; }

        public int MemberId { get; set; }
        public int RefereeId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Phone { get; set; }

        public int CourseId { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Ground { get; set; }

        [StringLength(100)]
        public string School { get; set; }

        [StringLength(20)]
        public string IdCard { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public decimal Amount { get; set; }
        public EnrollStatus Status { get; set; }
        public DateTime? PaidTime { get; set; }

        [StringLength(200)]
        public string PayType { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public DateTime UpdateTime { get; set; }

        public Enroll()
        {
            UpdateTime = DateTime.Now;
        }
    }
}