using System;
using System.Linq;
using System.Web;

namespace JerryPlat.Utils.Helpers
{
    public static class CookieHelper
    {
        #region public Cookie Function

        public static HttpCookie GetCookie(string strKey)
        {
            return HttpContext.Current.Response.Cookies[strKey];
        }

        public static HttpCookie GetCookie(string strName, string strValue, string strDomain, DateTime expiresDate, string strPath = "/")
        {
            HttpCookie httpCookie = new HttpCookie(strName, strValue);
            if (!string.IsNullOrEmpty(strDomain))
            {
                httpCookie.Domain = strDomain;
            }
            httpCookie.Expires = expiresDate;
            httpCookie.Path = strPath;
            return httpCookie;
        }

        public static void SetCookie(string strKey, string strValue, string strDomain = "", DateTime? expiresDate = null, string strPath = "/")
        {
            expiresDate = expiresDate ?? DateTime.Now.AddHours(7);
            HttpCookie cookie = GetCookie(strKey, strValue, strDomain, expiresDate.Value, strPath);
            HttpContext.Current.Response.SetCookie(cookie);
        }

        public static void SetCookie<T>(string strKey, T obj, string strDomain = "", DateTime? expiresDate = null, string strPath = "/") where T : class
        {
            string strValue = EncryptHelper.Encrypt(SerializationHelper.ToJson(obj));
            expiresDate = expiresDate ?? DateTime.Now.AddHours(7);
            SetCookie(strKey, strValue, strDomain, expiresDate.Value, strPath);
        }

        public static string GetCookieValue(string strKey)
        {
            return HttpContext.Current.Response.Cookies[strKey].Value;
        }

        public static T GetCookieValue<T>(string strKey) where T : class
        {
            string strEncryptValue = GetCookieValue(strKey);
            return SerializationHelper.JsonToObject<T>(EncryptHelper.Decrypt(strEncryptValue));
        }

        public static bool IsExistCookie(string strKey)
        {
            return HttpContext.Current.Response.Cookies.AllKeys.Contains(strKey);
        }

        public static void ClearCookie(string strKey)
        {
            HttpContext.Current.Response.Cookies.Get(strKey).Expires = DateTime.Now.AddDays(-1);
        }

        #endregion public Cookie Function
    }
}