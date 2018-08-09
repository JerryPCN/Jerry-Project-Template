using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace JerryPlat.Web.App_Start.Filter
{
    public class CompressAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //如果出现错误，则不进行压缩，否则页面会出现乱码，而不是报错的黄页
            if (filterContext.Exception != null)
                return;

            HttpResponseBase Response = filterContext.HttpContext.Response as HttpResponseBase;

            //判断IIS或者其他承载设备是是否启用了GZip或DeflateStream
            if (Response.Filter is GZipStream || Response.Filter is DeflateStream)
                return;

            //开始进入压缩环节
            string AcceptEncoding = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(AcceptEncoding))
            {
                if (AcceptEncoding.Contains("gzip"))
                {
                    Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);
                    Response.Headers.Remove("Content-Encoding");
                    Response.AppendHeader("Content-Encoding", "gzip");
                }
                else //if (AcceptEncoding.Contains("deflate"))
                {
                    Response.Filter = new DeflateStream(Response.Filter, CompressionMode.Compress);
                    Response.Headers.Remove("Content-Encoding");
                    Response.AppendHeader("Content-Encoding", "deflate");
                }
            }
        }
    }
}