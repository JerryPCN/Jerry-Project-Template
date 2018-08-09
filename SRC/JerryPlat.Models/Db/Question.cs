using JerryPlat.Utils.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JerryPlat.Models
{
    public class Question : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuestionTypeId { get; set; }

        [Required]
        public int QuestionChapterId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(200)]
        public string PicPath { get; set; }

        [Required]
        public int Mark { get; set; }

        [Required]
        public AnswerType AnswerType { get; set; }

        [Required]
        public int OrderIndex { get; set; }

        [StringLength(2000)]
        [AllowHtml]
        public string Explanation { get; set; }
    }
}