using System;

namespace JerryPlat.Models.Dto
{
    public class MemberScoreDto
    {
        public Member Member { get; set; }
        public int FirstCount { get; set; }
        public int SecondCount { get; set; }
        public DateTime? LastTime { get; set; }
    }
}