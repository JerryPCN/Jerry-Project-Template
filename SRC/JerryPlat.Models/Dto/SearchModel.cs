using JerryPlat.Utils.Models;
using System;

namespace JerryPlat.Models.Dto
{
    public class SearchModel : BaseSearchModel
    {
        public int Id1 { get; set; }
        public string SearchText { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}