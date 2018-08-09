using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JerryPlat.Web.App_Start.Filter
{
    public class LoginAuthorizeAttribute : AuthorizeAttribute
    {
        private string _Area { get; set; }
        private string _LoginUri { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.RequestContext.RouteData.DataTokens.Keys.Contains("area"))
            {
                return true;
            }

            _Area = httpContext.Request.RequestContext.RouteData.DataTokens["area"].ToString();

            if (!SessionHelper.KeyValues.Keys.Contains(_Area))
            {
                return true;
            }

            SessionHelper sessionHelper = SessionHelper.KeyValues[_Area];

            _LoginUri = sessionHelper.LoginUri;

            return !sessionHelper.IsNullSession();
        }

        //加上如下的重写方法，在方法内进行跳转，问题解决
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            string url = $"{_LoginUri}?returnURL=";
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                url += HttpUtility.UrlEncode(filterContext.HttpContext.Request.UrlReferrer.ToString());
                filterContext.Result = new JsonResult()
                {
                    Data = ResponseModel<string>.Logout(url),
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                url += HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString());
                filterContext.Result = new RedirectResult(url);
            }
        }
    }
}