namespace JerryPlat.Models.Dto
{
    public class TeamDto
    {
        /// <summary>
        /// 直接下级
        /// </summary>
        public Member Member { get; set; }

        /// <summary>
        /// 报名总人数（支付+未支付）
        /// </summary>
        public int TotalMember { get; set; }

        /// <summary>
        /// 已支付的人数
        /// </summary>
        public int PaidMember { get; set; }

        /// <summary>
        /// 佣金
        /// </summary>
        public int TotalScore { get; set; }

        /// <summary>
        /// 提现
        /// </summary>
        public int WithdrawScore { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
    }
}