using JerryPlat.Utils.Models;
using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models
{
    public class WxTicket : IEntity
    {
        [Key]
        public int Id { get; set; }

        public WxTicketType Key { get; set; }
        public string Ticket { get; set; }
        public int ExpireTime { get; set; }
    }
}