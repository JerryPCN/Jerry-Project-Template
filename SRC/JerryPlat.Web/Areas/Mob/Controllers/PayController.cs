using JerryPlat.BLL.CommonMagage;
using JerryPlat.Utils.Helpers;
using JerryPlat.Web.Areas.Base;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Mob.Controllers
{
    public class PayController : BaseController<OwinConfigHelper>
    {
        public ActionResult Login(string returnURL, string id)
        {
            string strResult = (string.IsNullOrEmpty(returnURL) || returnURL == "null") ? "/Mob" : HttpUtility.UrlDecode(returnURL);
            SessionHelper.MobReturnUrl.SetSession(strResult);
#if DEBUG
            if (_helper.Login())
            {
                return Redirect(strResult);
            }
#endif
            return Redirect(_helper.GetRequestUri(id));
        }

        // GET: Mob/Owin
        public async Task<ActionResult> Wechat(string code, string state)
        {
            if (!string.IsNullOrEmpty(code) && await _helper.Login("WeChat", state, code))
            {
                string strResult = SessionHelper.MobReturnUrl.GetSession<string>();
                if (string.IsNullOrEmpty(strResult))
                {
                    strResult = "/Mob";
                }

                return Redirect(strResult);
            }
            return Content("授权失败，请重试！");
        }
    }
}