using JerryPlat.Utils.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JerryPlat.Models
{
    public class QuestionType : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Source { get; set; }

        [Required]
        public int OrderIndex { get; set; }

        [NotMapped()]
        public int QuestionCount { get; set; }
    }
}