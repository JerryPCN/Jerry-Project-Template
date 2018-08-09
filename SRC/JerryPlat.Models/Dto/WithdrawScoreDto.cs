namespace JerryPlat.Models.Dto
{
    public class WithdrawScoreDto
    {
        public int Score { get; set; }
        public decimal TotalWithdrawApplyScore { get; set; }
        public decimal TotalWithdrawDiscardScore { get; set; }
        public decimal TotalWithdrawScore { get; set; }
        public decimal TotalTaxScore { get; set; }

        public decimal TotalScore
        {
            get
            {
                return TotalWithdrawScore - TotalTaxScore;
            }
        }
    }
}