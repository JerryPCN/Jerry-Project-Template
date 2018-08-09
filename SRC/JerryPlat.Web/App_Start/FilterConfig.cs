using JerryPlat.Web.App_Start.Filter;
using System.Web.Mvc;

namespace JerryPlat.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomHandleErrorAttribute());
            //filters.Add(new PreventCSRFAttribute());
            filters.Add(new ValidateModelAttribute());
            filters.Add(new CompressAttribute());
        }
    }
}