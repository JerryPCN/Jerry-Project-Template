using JerryPlat.Utils.Helpers;
using JerryPlat.Web.Controllers;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace JerryPlat.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MvcHandler.DisableMvcResponseHeader = true;
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");           //Remove Server Header
            Response.Headers.Remove("X-AspNet-Version"); //Remove X-AspNet-Version Header
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            LogHelper.Error(ex);

            var httpStatusCode = (ex is HttpException) ? (ex as HttpException).GetHttpCode() : 500; //这里仅仅区分两种错误
            var httpContext = ((MvcApplication)sender).Context;
            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = httpStatusCode;

            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Index";

            //if (httpStatusCode == 404)
            //{
            routeData.Values["code"] = httpStatusCode;
            Controller controller = new ErrorController();
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            //}
        }
    }
}