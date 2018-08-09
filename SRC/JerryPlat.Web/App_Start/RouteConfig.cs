using System.Web.Mvc;
using System.Web.Routing;

namespace JerryPlat.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ResourceJs",
                url: "Resource/{name}.js",
                defaults: new { controller = "Resource", action = "Js" },
                namespaces: new string[] { "JerryPlat.Web.Controllers" }
            );

            routes.MapRoute(
                name: "ResourceCss",
                url: "Resource/{name}.css",
                defaults: new { controller = "Resource", action = "Css" },
                namespaces: new string[] { "JerryPlat.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Error",
                url: "Error/{code}",
                defaults: new { controller = "Error", action = "Index" },
                namespaces: new string[] { "JerryPlat.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "JerryPlat.Web.Controllers" }
            );
        }
    }
}