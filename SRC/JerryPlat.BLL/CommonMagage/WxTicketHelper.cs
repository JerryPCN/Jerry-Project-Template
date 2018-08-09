using JerryPlat.DAL;
using JerryPlat.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JerryPlat.BLL.CommonMagage
{
    public class WxTicketHelper : BaseHelper
    {
        public async Task<WxTicket> GetWxTicketAsync(WxTicketType wxTicketType)
        {
            return await _Db.WxTickets.Where(o => o.Key == wxTicketType).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAsync(WxTicketType wxTicketType, WxTicket ticket)
        {
            WxTicket wxTicket = await GetWxTicketAsync(wxTicketType);
            if (wxTicket == null)
            {
                wxTicket = new WxTicket();
                wxTicket.Key = wxTicketType;
            }
            wxTicket.Ticket = ticket.Ticket;
            wxTicket.ExpireTime = ticket.ExpireTime;
            return await base.SaveAsync(wxTicket);
        }
    }
}