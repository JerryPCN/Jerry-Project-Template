using JerryPlat.Utils.Helpers;
using System.Web.Mvc;

namespace JerryPlat.Web.Controllers
{
    //[OutputCache(Duration = 60 * 60 * 24)]//设置过期时间单位为秒
    public class ResourceController : Controller
    {
        // GET: Resource
        public ActionResult Js(string name)
        {
            string strScript = string.Empty;
            if ("Constant" == name)
            {
                strScript = ConstantHelper.GetScript();
            }

            return JavaScript(strScript);
        }

        // GET: Resource
        public ActionResult Css(string name)
        {
            return Content("");
        }
    }
}