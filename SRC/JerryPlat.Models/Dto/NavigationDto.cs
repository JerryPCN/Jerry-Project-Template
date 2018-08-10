using System.Collections.Generic;

namespace JerryPlat.Models.Dto
{
    public class NavigationDto
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public int ParentId { get; set; }
        public int OrderIndex { get; set; }
        public SiteType SiteType { get; set; }
        public List<Navigation> Children { get; set; }
    }
}