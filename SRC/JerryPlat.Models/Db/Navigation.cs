using JerryPlat.Utils.Models;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public enum SiteType
    {
        Admin = 1,
        Web = 2
    }

    public class Navigation : IEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string PageName { get; set; }

        [StringLength(200)]
        public string PageUrl { get; set; }

        [Required]
        public int ParentId { get; set; }

        [Required]
        public int OrderIndex { get; set; }

        [Required]
        public SiteType SiteType { get; set; }
    }
}