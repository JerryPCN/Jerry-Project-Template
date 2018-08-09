using JerryPlat.Utils.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JerryPlat.Models
{
    public class QuestionChapter : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuestionTypeId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int OrderIndex { get; set; }

        [NotMapped()]
        public int QuestionCount { get; set; }
    }
}