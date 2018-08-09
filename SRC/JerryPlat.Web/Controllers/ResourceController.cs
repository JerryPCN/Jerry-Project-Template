using JerryPlat.Utils.Helpers;
using System.Web.Mvc;

namespace JerryPlat.Web.Controllers
{
    //[OutputCache(Duration = 2000)]//设置过期时间单位为秒
    public class ResourceController : Controller
    {
        // GET: Resource
        [OutputCache()]
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
        [OutputCache()]
        public ActionResult Css(string name)
        {
            return Content("");
        }
    }
}