using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JerryPlat.Utils.Helpers
{
    public class SessionHelper
    {
        public static SessionHelper Instance = GetInstance<SessionHelper>("Instance");
        public static SessionHelper Admin = GetInstance<SessionHelper>("Admin", "/Admin/Home/Index");
        public static SessionHelper Mob = GetInstance<SessionHelper>("Mob", "/Mob/Owin/Login/Wechat");
        public static SessionHelper MobReturnUrl = GetInstance<SessionHelper>("MobReturnUrl");
        public static SessionHelper Owin = GetInstance<SessionHelper>("Owin");
        public static SessionHelper ShareCode = GetInstance<SessionHelper>("ShareCode");
        public static SessionHelper SMS = GetInstance<SessionHelper>("SMS");
        public static SessionHelper User = GetInstance<SessionHelper>("User");
        public static SessionHelper VerifyCode = GetInstance<SessionHelper>("VerifyCode");

        public static Dictionary<string, SessionHelper> KeyValues = new Dictionary<string, SessionHelper>() {
            {"Admin", Admin },
            {"Mob", Mob }
        };

        public string LoginUri { get; set; }

        public static T GetInstance<T>(string strSessionName, string strLoginUri = "") where T : SessionHelper, new()
        {
            T sessionHelper = new T();
            sessionHelper._SessionName = "JerryPlat_SessionName_" + strSessionName;
            sessionHelper.LoginUri = strLoginUri;
            return sessionHelper;
        }

        #region Private Functions

        private static bool IsNullSession(string strKey)
        {
            return HttpContext.Current.Session[strKey] == null;
        }

        private static T GetSession<T>(string strKey)
        {
            if (IsNullSession(strKey))
            {
                return default(T);
            }

            return (T)HttpContext.Current.Session[strKey];
        }

        private static void SetSession(string strSessionKey, object objValue, int timeout = 0)
        {
            if (!KeyValues.Keys.Contains(strSessionKey))
            {
                throw new Exception("Not Exist Session with Key = " + strSessionKey);
            }

            KeyValues[strSessionKey].SetSession(objValue, timeout);
        }

        #endregion Private Functions

        #region Default Session Name

        private string _SessionName { get; set; }

        public virtual string GetDefaultSessionName()
        {
            return _SessionName;
        }

        public bool IsNullSession()
        {
            string strDefaultSessionName = GetDefaultSessionName();
            return HttpContext.Current.Session[strDefaultSessionName] == null;
        }

        public T GetSession<T>()
        {
            string strDefaultSessionName = GetDefaultSessionName();
            return GetSession<T>(strDefaultSessionName);
        }

        public void SetSession(object objValue, int timeout = 0)
        {
            string strDefaultSessionName = GetDefaultSessionName();

            HttpContext.Current.Session[strDefaultSessionName] = objValue;
            if (timeout > 0)
            {
                HttpContext.Current.Session.Timeout = timeout;
            }
        }

        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        public bool IsValid(string strCode)
        {
            return GetSession<string>().Equals(strCode, StringComparison.OrdinalIgnoreCase);
        }

        #endregion Default Session Name
    }
}