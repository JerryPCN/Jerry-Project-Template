using JerryPlat.Utils.Helpers;
using NPOI.Extension;
using System;

namespace JerryPlat.Models.Dto
{
    public class EnrollExportDto
    {
        [Column(Index = 17, IsIgnored = true)]
        public int Id { get; set; }

        [Column(Index = 0, Title = "推荐码")]
        public string ShareCode { get; set; }

        [Column(Index = 1, Title = "订单号")]
        public string OrderNo { get; set; }

        [Column(Index = 18, IsIgnored = true)]
        public int MemberId { get; set; }

        [Column(Index = 19, IsIgnored = true)]
        public int RefereeId { get; set; }

        [Column(Index = 2, Title = "申请人")]
        public string Name { get; set; }

        [Column(Index = 4, Title = "电话")]
        public string Phone { get; set; }

        [Column(Index = 20, IsIgnored = true)]
        public int CourseId { get; set; }

        [Column(Index = 6, Title = "城市")]
        public string City { get; set; }

        [Column(Index = 7, Title = "场地")]
        public string Ground { get; set; }

        [Column(Index = 8, Title = "学校")]
        public string School { get; set; }

        [Column(Index = 9, Title = "身份证号")]
        public string IdCard { get; set; }

        [Column(Index = 10, Title = "地址")]
        public string Address { get; set; }

        [Column(Index = 11, Title = "报名费用")]
        public decimal Amount { get; set; }

        [Column(Index = 21, IsIgnored = true)]
        public EnrollStatus Status { get; set; }

        [Column(Index = 13, Title = "支付时间")]
        public DateTime? PaidTime { get; set; }

        [Column(Index = 14, Title = "支付方式")]
        public string PayType { get; set; }

        [Column(Index = 15, Title = "备注")]
        public string Note { get; set; }

        [Column(Index = 16, Title = "报名时间")]
        public DateTime UpdateTime { get; set; }

        [Column(Index = 5, Title = "课程")]
        public string Course { get; set; }

        [Column(Index = 3, Title = "推荐人")]
        public string RefereeName { get; set; }

        [Column(Index = 12, Title = "支付状态")]
        public string PayStatus { get { return Status.GetDescription(); } }
    }
}