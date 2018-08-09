using JerryPlat.Utils.Models;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class Coach : IEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public Sex Sex { get; set; }

        [StringLength(200)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Summary { get; set; }

        [StringLength(200)]
        public string PicPath { get; set; }

        public int OrderIndex { get; set; }
    }
}