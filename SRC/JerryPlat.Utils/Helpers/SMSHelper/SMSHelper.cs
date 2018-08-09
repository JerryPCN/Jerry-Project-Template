using JerryPlat.Utils.Models;
using System;

namespace JerryPlat.Utils.Helpers
{
    public class SMSHelper
    {
        private static string GetCode()
        {
            //return "1234";
            Random random = new Random();
            return (random.Next(9000) + 1000).ToString();
        }

        public static bool SendCode(string strPhone, out string strMsg)
        {
            string strCode = GetCode();
            string strContent = SystemConfigModel.Instance.SmsCodeTemplate.Replace("{{Code}}", strCode);
            bool bReturn = SendContent(strPhone, strContent, out strMsg);
            if (bReturn)
            {
                SessionHelper.SMS.SetSession(strCode);
            }
            return bReturn;
        }

        public static bool SendPay(string strPhone, decimal decAmount, string strWithdrawType, out string strMsg)
        {
            if (!SystemConfigModel.Instance.IsUseSms)
            {
                strMsg = "SMS is not enabled.";
                return true;
            }

            string strContent = SystemConfigModel.Instance.SmsPayTemplate
                .Replace("{{Amount}}", decAmount.ToString())
                .Replace("{{WithdrawType}}", strWithdrawType);
            bool bReturn = SendContent(strPhone, strContent, out strMsg);
            return bReturn;
        }

        public static bool SendContent(string strPhone, string strContent, out string strMsg)
        {
            //return true;
            SMS sms = new SMS();
            if (sms.Send(strPhone, strContent, out strMsg))
            {
                return true;
            }

            return false;
        }

        public static bool IsValid(string strCode)
        {
            return SessionHelper.SMS.IsValid(strCode);
        }
    }
}