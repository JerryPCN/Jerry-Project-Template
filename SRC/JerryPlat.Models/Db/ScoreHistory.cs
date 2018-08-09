using JerryPlat.Utils.Models;
using NPOI.Extension;
using System;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class ScoreHistory : IEntity
    {
        [Key]
        [Column(Index = 3, IsIgnored = true)]
        public int Id { get; set; }

        [Column(Index = 4, IsIgnored = true)]
        public int MemberId { get; set; }

        [Column(Index = 0, Title = "积分变动")]
        public int Different { get; set; }

        [Column(Index = 1, Title = "变动描述")]
        public string Description { get; set; }

        [Column(Index = 2, Title = "变动时间", Formatter = "yyyy-MM-dd HH:mm:ss")]
        public DateTime UpdateTime { get; set; }

        public ScoreHistory()
        {
            UpdateTime = DateTime.Now;
        }
    }
}