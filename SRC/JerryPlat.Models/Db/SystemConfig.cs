using JerryPlat.Utils.Models;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class SystemConfig : IEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Config { get; set; }
    }
}