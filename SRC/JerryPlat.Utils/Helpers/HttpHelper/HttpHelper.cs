using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace JerryPlat.Utils.Helpers
{
    public class HttpHelper
    {
        #region URL请求数据

        public static T Post<T>(string url, string param)
            where T : class
        {
            return SerializationHelper.JsonToObject<T>(Post(url, param));
        }

        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string Post(string url, string param)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }

        public static T Get<T>(string url)
            where T : class
        {
            return SerializationHelper.JsonToObject<T>(Get(url));
        }

        public static Stream GetStream(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 15000;
            request.Proxy = null;
            request.Method = "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
            request.UserAgent = "Mozilla/5.0 (Windows NT 5.2; rv:12.0) Gecko/20100101 Firefox/12.0";

            WebResponse response = null;
            Stream responseStream = null;

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    responseStream = response.GetResponseStream();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }

            return responseStream;
        }

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            WebResponse response = null;
            string responseStr = null;

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }

            return responseStr;
        }

        /// <summary>
        /// 执行URL获取页面内容
        /// </summary>
        public static string UrlExecute(string urlPath)
        {
            if (string.IsNullOrEmpty(urlPath))
            {
                return "error";
            }
            StringWriter sw = new StringWriter();
            try
            {
                HttpContext.Current.Server.Execute(urlPath, sw);
                return sw.ToString();
            }
            catch (Exception)
            {
                return "error";
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }
        }

        #endregion URL请求数据

        public static string GetLocalDomain()
        {
            string scheme = HttpContext.Current.Request.Url.Scheme;
            string host = HttpContext.Current.Request.Url.Host;
            return scheme + "://" + host;
        }

        public static bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            return (((url[0] == '/') && ((url.Length == 1) || ((url[1] != '/') && (url[1] != '\\')))) || (((url.Length > 1) && (url[0] == '~')) && (url[1] == '/')))
                || url.StartsWith(GetLocalDomain());
        }
    }
}