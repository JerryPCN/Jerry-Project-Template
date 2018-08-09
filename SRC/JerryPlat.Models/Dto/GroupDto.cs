using System.Collections.Generic;

namespace JerryPlat.Models.Dto
{
    public class GroupDto
    {
        public Group Group { get; set; }
        public List<int> NavigationIdList { get; set; }

        public GroupDto()
        {
            Group = new Group();
            NavigationIdList = new List<int>();
        }
    }
}