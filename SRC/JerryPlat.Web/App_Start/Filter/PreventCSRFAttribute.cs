using System;
using System.Web.Mvc;

namespace JerryPlat.Web.App_Start.Filter
{
    public class PreventCSRFAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext.Request.RequestType == "POST")
            {
                ValidateAntiForgeryTokenAttribute token = new ValidateAntiForgeryTokenAttribute();
                token.OnAuthorization(_filterAuthorizationContext);
            }
        }

        private AuthorizationContext _filterAuthorizationContext = null;

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            _filterAuthorizationContext = filterContext;
        }
    }
}