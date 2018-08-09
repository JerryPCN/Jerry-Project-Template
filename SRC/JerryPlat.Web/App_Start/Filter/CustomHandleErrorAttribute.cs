using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace JerryPlat.Web.App_Start.Filter
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            LogHelper.Error(filterContext.Exception);

            string strErrorMsg = GetExceptionType(filterContext.Exception);

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                JsonNetResult jsonResult = new JsonNetResult
                {
                    Data = ResponseModel<string>.Error(strErrorMsg),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.Result = jsonResult;
                return;
            }

            var result = new ViewResult()
            {
                ViewName = "Error"
            };
            result.ViewBag.Error = strErrorMsg;
            filterContext.Result = result;
        }

        private const string ERROR = "程序发生了错误，请先联系技术人员，或稍候再试。";
        private const string INVALID_REQUEST = "对不起，该请求非法，请合法的使用该站点。";

        private string GetExceptionType(Exception ex)
        {
            if (ex is HttpAntiForgeryException
                || ex is HttpRequestValidationException)
            {
                return INVALID_REQUEST;
            }

            return ERROR;
        }
    }
}