using System.Collections.Generic;

namespace JerryPlat.Models.Dto
{
    public class NavigationDto : Navigation
    {
        public List<Navigation> Children { get; set; }
    }
}