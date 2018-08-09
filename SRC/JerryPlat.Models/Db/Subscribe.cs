using JerryPlat.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class Subscribe : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int MemberId { get; set; }
        public int CourseId { get; set; }
        public int GroundId { get; set; }
        public int CoachId { get; set; }
        public DateTime TeachTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public Subscribe()
        {
            UpdateTime = DateTime.Now;
        }
    }
}