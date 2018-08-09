using JerryPlat.Utils.Models;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace JerryPlat.Web.App_Start.Filter
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var viewData = actionContext.Controller.ViewData;
            var modelState = viewData.ModelState;
            if (!modelState.IsValid)
            {
#if DEBUG
                StringBuilder sbError = new StringBuilder();
                sbError.AppendLine("<p>Please note the errors below:</p>");
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        sbError.AppendLine($"<p>{state.Errors.First().ErrorMessage}</p>");
                    }
                }

                actionContext.Result = new JsonResult
                {
                    Data = ResponseModel<string>.Invalid(sbError.ToString()),
                    ContentEncoding = Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
#else
                actionContext.Result = new JsonResult
                {
                    //Data = ResponseModel<string>.Invalid<string>(sbError.ToString()),
                    Data = ResponseModel<string>.Invalid("请先填写有效的数据。"),
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
#endif
            }
        }
    }
}