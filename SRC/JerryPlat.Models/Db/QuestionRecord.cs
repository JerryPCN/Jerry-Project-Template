using JerryPlat.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public enum PracticeType
    {
        Order = 1,
        Random = 2,
        Favorite = 3,
        Error = 4,
        Ignore = 5,
        Exam = 6
    }

    public class QuestionRecord : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int MemberId { get; set; }
        public int QuestionTypeId { get; set; }
        public int QuestionChapterId { get; set; }
        public int ExamId { get; set; }
        public PracticeType PracticeType { get; set; }
        public int CurrentIndex { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}