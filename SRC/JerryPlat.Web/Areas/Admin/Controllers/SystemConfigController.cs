using JerryPlat.BLL.AdminManage;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class SystemConfigController : AuthurizeBaseController<SystemConfigHelper>
    {
        [HttpPost]
        public ActionResult Get()
        {
            return Success(_helper.Get());
        }

        [HttpPost]
        public ActionResult Save(SystemConfigModel model)
        {
            _helper.Save(model);
            return Success();
        }
    }
}