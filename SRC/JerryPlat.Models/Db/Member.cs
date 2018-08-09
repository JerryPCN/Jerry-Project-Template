using JerryPlat.Models.Dto;
using JerryPlat.Utils.Models;
using NPOI.Extension;
using System;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class Member : MemberDto, IEntity
    {
        [Key]
        [Column(Index = 0)]
        public int Id { get; set; }

        [StringLength(200)]
        [Column(Index = 1)]
        public string OpenId { get; set; }

        [StringLength(200)]
        [Column(Index = 2, Title = "图像地址")]
        public string Avatar { get; set; }

        [StringLength(200)]
        [Column(Index = 14, IsIgnored = true, Title = "密码")]
        public string Password { get; set; }

        [Column(Index = 3, Title = "性别")]
        public Sex Sex { get; set; }

        [StringLength(200)]
        [Column(Index = 4, Title = "电话")]
        public string Phone { get; set; }

        [StringLength(200)]
        [Column(Index = 5, Title = "姓名")]
        public string Name { get; set; }

        [Column(Index = 6, Title = "推荐人ID")]
        public int RefereeId { get; set; }

        [Column(Index = 7, Title = "优币")]
        public int Score { get; set; }

        [Column(Index = 8, Title = "加入时间", Formatter = "yyyy-MM-dd HH:mm:ss")]
        public DateTime JoinTime { get; set; }

        [StringLength(200)]
        [Column(Index = 9, Title = "推荐码")]
        public string ShareCode { get; set; }

        public Member()
        {
            JoinTime = DateTime.Now;
        }
    }
}