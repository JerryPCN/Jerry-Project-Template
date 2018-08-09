using JerryPlat.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class PayRecord : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int MemberId { get; set; }

        [StringLength(200)]
        public string OrderNo { get; set; }

        public decimal Amount { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public string PayType { get; set; }
        public DateTime UpdateTime { get; set; }

        public PayRecord()
        {
            UpdateTime = DateTime.Now;
        }
    }
}