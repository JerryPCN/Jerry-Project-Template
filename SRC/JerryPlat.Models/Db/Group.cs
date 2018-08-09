using JerryPlat.Utils.Models;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class Group : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }
    }
}