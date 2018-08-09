using JerryPlat.Utils.Models;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class QuestionFavorite : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int MemberId { get; set; }
        public int QuestionTypeId { get; set; }
        public int QuestionChapterId { get; set; }
        public int QuestionId { get; set; }
        public bool IsFavorite { get; set; }
    }
}