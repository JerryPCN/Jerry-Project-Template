using System;

namespace JerryPlat.Pay.WxPay
{
    public class WxPayException : Exception
    {
        public WxPayException(string msg) : base(msg)
        {
        }
    }
}