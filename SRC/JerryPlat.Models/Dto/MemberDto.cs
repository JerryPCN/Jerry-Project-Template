using NPOI.Extension;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models.Dto
{
    public class MemberDto
    {
        [StringLength(200)]
        [Column(Index = 10, Title = "昵称")]
        public string NickName { get; set; }

        [Column(Index = 11, IsIgnored = true, Title = "纬度")]
        public float? Latitude { get; set; }

        [Column(Index = 12, IsIgnored = true, Title = "经度")]
        public float? Longitude { get; set; }

        [Column(Index = 13, IsIgnored = true, Title = "精确度")]
        public float? Precision { get; set; }
    }
}