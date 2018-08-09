using JerryPlat.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class Exam : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int MemberId { get; set; }
        public int QuestionTypeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsEnd { get; set; }
        public int TotalMark { get; set; }
    }
}