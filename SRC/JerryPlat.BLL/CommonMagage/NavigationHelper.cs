using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JerryPlat.BLL.CommonMagage
{
    public class NavigationHelper : BaseHelper
    {
        public Task<List<Navigation>> GetListAsync(SiteType siteType, int intGroupId = 0)
        {
            IQueryable<Navigation> navigationQuerable = _Db.Navigations.AsNoTracking().Where(o => o.SiteType == siteType);

            if (intGroupId == 0)
            {
                return navigationQuerable.ToListAsync();
            }

            return (from a in _Db.Roles.Where(o => o.GroupId == intGroupId)
                    join b in navigationQuerable on a.NavigationId equals b.Id
                    select b).ToListAsync();
        }

        public List<NavigationDto> GetTreeListAsync(SiteType siteType)
        {
            List<Navigation> navigationList = _Db.Navigations.AsNoTracking().Where(o => o.SiteType == siteType).OrderBy(o => o.OrderIndex).ToList();
            return (from a in navigationList.Where(o => o.ParentId == 0)
                    let b = navigationList.Where(o => o.ParentId == a.Id).OrderBy(o => o.OrderIndex).ToList()
                    select new NavigationDto
                    {
                        Id = a.Id,
                        PageName = a.PageName,
                        PageUrl = a.PageUrl,
                        ParentId = a.ParentId,
                        OrderIndex = a.OrderIndex,
                        SiteType = a.SiteType,
                        Children = b
                    }).ToList();
        }
    }
}