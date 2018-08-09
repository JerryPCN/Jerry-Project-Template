using JerryPlat.Utils.Models;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    //http://www.cnblogs.com/txw1958/p/3415145.html
    public class OwinConfig : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string RequestUri { get; set; }

        [Required]
        [StringLength(500)]
        public string AccessTokenUri { get; set; }

        [Required]
        [StringLength(500)]
        public string UserInfoUri { get; set; }

        [Required]
        [StringLength(200)]
        //公众号的唯一标识
        public string AppId { get; set; }

        [Required]
        [StringLength(200)]
        //公众号的appsecret
        public string AppSecret { get; set; }

        [Required]
        [StringLength(200)]
        //授权后重定向的回调链接地址
        public string RedirectUri { get; set; }
    }
}