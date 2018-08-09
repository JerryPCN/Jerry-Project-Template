using JerryPlat.BLL.UserManage;
using JerryPlat.Utils.Helpers;
using JerryPlat.Web.Areas.Base;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class SystemController : AuthurizeBaseController
    {
        [HttpPost]
        public async Task<ActionResult> ClearData()
        {
            AdminUserHelper helper = new AdminUserHelper();
            await helper.ClearData();
            return Restart();
        }

        [HttpPost]
        public ActionResult Restart()
        {
            SiteHelper.RestartAppDomain();
            return Success();
        }
    }
}