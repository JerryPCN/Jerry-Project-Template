using JerryPlat.Utils.Models;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class Role : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int NavigationId { get; set; }
    }
}