using JerryPlat.BLL.CommonMagage;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using JerryPlat.Web.Areas.Base;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Mob.Controllers
{
    //http://www.cnblogs.com/txw1958/p/3415145.html
    public class OwinController : BaseController<OwinConfigHelper>
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
            if (!SessionHelper.Mob.IsNullSession()
                || (!string.IsNullOrEmpty(code) && await _helper.Login("WeChat", state, code)))
            {
                Member member = SessionHelper.Mob.GetSession<Member>();
                string strResult = string.Empty;
                //if (string.IsNullOrEmpty(member.Phone))
                //{
                //    strResult = "/Mob/Home/Bind";
                //}
                //else
                //{
                strResult = SessionHelper.MobReturnUrl.GetSession<string>();
                if (string.IsNullOrEmpty(strResult))
                {
                    strResult = "/Mob";
                }
                //}

                return Redirect(strResult);
            }
            return Content("授权失败，请重试！");
        }
    }
}