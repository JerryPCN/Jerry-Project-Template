namespace JerryPlat.Models.Dto
{
    public class WithdrawDto
    {
        public Withdraw Withdraw { get; set; }
        public Member Member { get; set; }
        public string WithdrawType { get; set; }
        public string WithdrawStatus { get; set; }
    }
}