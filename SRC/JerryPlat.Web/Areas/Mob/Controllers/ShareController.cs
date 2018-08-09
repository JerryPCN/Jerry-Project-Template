using JerryPlat.Models;
using JerryPlat.Pay.WxPay;
using JerryPlat.Utils.Helpers;
using JerryPlat.Web.Areas.Base;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Mob.Controllers
{
    public class ShareController : BaseController
    {
        // GET: Mob/Share
        public async Task<ActionResult> Index(string id = "")
        {
            if (SessionHelper.Mob.IsNullSession())
            {
                return Redirect("/Mob/Home/Index/" + id);
            }

            if (string.IsNullOrEmpty(id))
            {
                Member member = SessionHelper.Mob.GetSession<Member>();
                if (member != null)
                {
                    id = member.ShareCode;
                }
            }

            WxPayData data = await WxPayApi.GetJSSDKConfig();
            ViewBag.WxConfig = data == null ? "" : SerializationHelper.ToJson(data.GetValues());

            return View((object)id);
        }

        public ActionResult Code(string id)
        {
            SessionHelper.ShareCode.SetSession(id);
            return Redirect("/Mob/Home/Index/" + id);
        }
    }
}