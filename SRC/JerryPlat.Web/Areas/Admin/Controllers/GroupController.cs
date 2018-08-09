using JerryPlat.BLL.CommonMagage;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class GroupController : AuthurizeBaseController<GroupHelper>
    {
        [HttpPost]
        public ActionResult GetNavigationTreeList()
        {
            return Success(new NavigationHelper().GetTreeListAsync(Models.SiteType.Admin));
        }

        [HttpPost]
        public ActionResult GetList(SearchModel search, PageParam pageParam)
        {
            PageData<Group> pageData = _helper.GetPageList<Group, SearchModel>(search, pageParam, true);
            return Success(pageData);
        }

        [HttpPost]
        public ActionResult GetPageList(SearchModel search, PageParam pageParam)
        {
            PageData<GroupDto> pageData = _helper.GetPageList(search, o => o.Name, pageParam, true);
            return Success(pageData);
        }

        [HttpPost]
        public async Task<ActionResult> Save(GroupDto model)
        {
            bool result = await _helper.SaveAsync(model);
            return Success(result);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            bool result = await _helper.DeleteAsync(id);
            return Success(result);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteList(List<int> idList)
        {
            bool result = await _helper.DeleteListAsync(idList);
            return Success(result);
        }
    }
}