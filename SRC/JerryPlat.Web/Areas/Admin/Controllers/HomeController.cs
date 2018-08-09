using JerryPlat.BLL.UserManage;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController<AdminUserHelper>
    {
        [HttpPost]
        public async Task<ActionResult> Login(string returnURL, LoginModel loginModel)
        {
            loginModel.Password = EncryptHelper.Encrypt(loginModel.Password);
            AdminUser user = await _helper.GetAsync(loginModel);
            if (user == null)
            {
                return Faild("您输入的用户名或密码错误。");
            }

            SessionHelper.Admin.SetSession(user);

            string strResult = "/Admin/Banner";

            if (!string.IsNullOrEmpty(returnURL) && returnURL != "null")
            {
                returnURL = HttpUtility.UrlDecode(returnURL);

                if (HttpHelper.IsLocalUrl(returnURL))
                {
                    strResult = returnURL;
                }
            }

            return Success(strResult);
        }

        /// <summary>
        /// 注销  删除session+cookie
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            SessionHelper.ClearSession();
            return Redirect("/Admin/Home/Index");
        }
    }
}