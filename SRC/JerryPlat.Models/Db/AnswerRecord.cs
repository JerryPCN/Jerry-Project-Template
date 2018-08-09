using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public enum AnswerStatus
    {
        Error = -1,
        Ignore = 0,
        Correct = 1
    }

    public class AnswerRecord : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int MemberId { get; set; }
        public int QuestionRecordId { get; set; }
        public int ExamId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [StringLength(200)]
        public string Answer { get; set; }

        public AnswerStatus AnswerStatus { get; set; }
        public DateTime UpdateTime { get; set; }

        public AnswerRecord()
        {
            UpdateTime = DateTime.Now;
        }

        public static implicit operator AnswerRecord(List<AnswerRecord> v)
        {
            throw new NotImplementedException();
        }
    }
}