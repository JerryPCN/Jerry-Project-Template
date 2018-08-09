using System;
using System.Text.RegularExpressions;
using System.Web;

namespace JerryPlat.Utils.Helpers
{
    public static class ToolHelper
    {
        public static string ToSpecialString(this string strText, int intStartIndex, int intLength, string strSpecial = "*")
        {
            string strReturn = string.Empty;
            int intLen = 0;
            for (int index = 0; index < strText.Length; index++)
            {
                if (index >= intStartIndex && intLen < intLength)
                {
                    strReturn += strSpecial;
                    intLen++;
                }
                else
                {
                    strReturn += strText[index];
                }
            }
            return strReturn;
        }

        #region 检查是否为IP地址

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result) || !IsIP(result))
            {
                return "127.0.0.1";
            }
            return result;
        }

        #endregion 检查是否为IP地址

        public static string GetOrderNo(Func<string, bool> IsExistOrderNo = null, string strProfix = "C", int intLength = 4)
        {
            DateTime dateTime = DateTime.Now;
            string strOrderNo = string.Empty;
            while (string.IsNullOrEmpty(strOrderNo) || (IsExistOrderNo != null && IsExistOrderNo(strOrderNo)))
            {
                strOrderNo = $"{strProfix}{dateTime.ToString("yyyyMMddHHmmss")}{VerifyCodeHelper.CreateRandomCode(intLength)}";
            }
            return strOrderNo;
        }

        public static decimal ToMultiply(this decimal decMultiply, string strMultiply)
        {
            decimal dec1 = 0m, dec2 = 1m;
            if (strMultiply.Contains("/") & strMultiply != "N/A")
            {
                string[] aryMultiply = strMultiply.Split(new char[] { '/' });
                if (aryMultiply.Length != 2
                    || !decimal.TryParse(aryMultiply[0], out dec1)
                    || !decimal.TryParse(aryMultiply[1], out dec2)
                    || dec2 == 0m
                    )
                {
                    throw new Exception("Illeagal Multiple:" + strMultiply);
                }
            }
            else if (strMultiply.EndsWith("%"))
            {
                if (!decimal.TryParse(strMultiply.TrimEnd('%'), out dec1))
                {
                    throw new Exception("Illeagal Multiple:" + strMultiply);
                }
                dec2 = 100m;
            }
            else
            {
                if (!string.IsNullOrEmpty(strMultiply) && strMultiply != "N/A" && !decimal.TryParse(strMultiply, out dec1))
                {
                    throw new Exception("Illeagal Multiple:" + strMultiply);
                }
            }
            return (decMultiply * dec1 / dec2);
        }

        public static DateTime GetMonthStartDate(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, 1);
        }

        public static string ToFormat(this DateTime datetime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return datetime.ToString(format);
        }
    }
}