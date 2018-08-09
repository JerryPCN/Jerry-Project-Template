using JerryPlat.Utils.Helpers;
using JerryPlat.Web.App_Start.Filter;
using JerryPlat.Web.Areas.Base;
using System.Web;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    [LoginAuthorize]
    public class UploadController : BaseController
    {
        // GET: Admin/Upload
        public ActionResult Index(HttpPostedFileBase file, string folder)
        {
            if (!file.IsValid())
            {
                return Faild("请上传合法的文件。");
            }

            return Success(file.SaveTo(folder));
        }
    }
}