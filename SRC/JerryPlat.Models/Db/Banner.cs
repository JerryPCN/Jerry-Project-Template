using JerryPlat.Utils.Models;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public enum BannerType
    {
        /// <summary>
        /// 移动端首页
        /// </summary>
        MobIndex = 1,

        /// <summary>
        /// 习题库首页
        /// </summary>
        QuestionIndex = 2,

        /// <summary>
        /// 题目页面
        /// </summary>
        Question = 3
    }

    public class Banner : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public BannerType BannerType { get; set; }

        [StringLength(200)]
        public string PicPath { get; set; }

        [StringLength(200)]
        public string PicUrl { get; set; }

        public int OrderIndex { get; set; }
    }
}