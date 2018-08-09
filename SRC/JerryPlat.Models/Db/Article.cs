using JerryPlat.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JerryPlat.Models
{
    public enum ArticleType
    {
        /// <summary>
        /// 头条
        /// </summary>
        TopLine = 1,

        /// <summary>
        /// 试学政策
        /// </summary>
        TryLearnPolicy = 2,

        /// <summary>
        /// 考场地址
        /// </summary>
        GroupAddress = 3,

        /// <summary>
        /// 学车流程
        /// </summary>
        LearnFlow = 4,

        /// <summary>
        /// 考试预约
        /// </summary>
        ExamOrder = 5
    }

    public class Article : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int AdminUserId { get; set; }

        [Required]
        public ArticleType ArticleType { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public DateTime UpdateTime { get; set; }

        public Article()
        {
            UpdateTime = DateTime.Now;
        }
    }
}