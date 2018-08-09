using JerryPlat.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class MemberScoreHistory : IEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime MonthTime { get; set; }
        public int AdminUserId { get; set; }
        public DateTime OperateTime { get; set; }
    }
}