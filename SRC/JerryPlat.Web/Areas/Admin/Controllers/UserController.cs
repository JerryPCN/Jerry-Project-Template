using JerryPlat.BLL.UserManage;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class UserController : AuthurizeBaseController<AdminUserHelper>
    {
        [HttpPost]
        public ActionResult GetList(SearchModel search, PageParam pageParam)
        {
            PageData<AdminUserDto> pageData = _helper.GetPageList(search, o => o.Id, pageParam, true);
            return Success(pageData);
        }

        [HttpPost]
        public async Task<ActionResult> Save(AdminUser model)
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

        [HttpPost]
        public async Task<ActionResult> ChangePassword(PasswordDto model)
        {
            bool result = await _helper.ChangePassword(model);
            return Success(result);
        }
    }
}