using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Pay.WxPay;
using JerryPlat.Utils.Helpers;
using JerryPlat.Web.Areas.Base;
using System;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Mob.Controllers
{
    public class WxPayController : BasePayController
    {
        protected override ActionResult SetPostRequest(Enroll enroll)
        {
            try
            {
                Member member = SessionHelper.Mob.GetSession<Member>();
                JsApiPay jsApiPay = new JsApiPay();
                jsApiPay.openid = member.OpenId;
                jsApiPay.GetUnifiedOrderResult(enroll, _helper.GetCourse(enroll.CourseId));
                WxPayData wxPayData = jsApiPay.GetJsApiParameters();
                return Success(wxPayData.GetValues());//获取H5调起JS API参数
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Faild("支付失败，请重试。");
            }
        }

        protected override EnrollStatus GetReturnEnrollStatus(string strOrderNo = "")
        {
            if (string.IsNullOrEmpty(strOrderNo))
            {
                strOrderNo = GetString("order_no");
            }

            if (string.IsNullOrEmpty(strOrderNo))
            {
                return EnrollStatus.PayFaild;
            }

            return IsPaid("", strOrderNo) ? EnrollStatus.PaySuccess : EnrollStatus.PayFaild;
        }

        protected override EnrollStatus GetNoticeEnrollStatus(string strOrderNo = "")
        {
            if (string.IsNullOrEmpty(strOrderNo))
            {
                WxPayData notifyData = WxPayApi.GetNotifyData();
                //检查支付结果中transaction_id是否存在
                if (!notifyData.IsSet("transaction_id"))
                {
                    //支付结果中微信订单号不存在
                    return EnrollStatus.PayFaild;
                }

                strOrderNo = notifyData.GetValue("transaction_id").ToString(); //微信支付订单号
            }

            //查询订单，判断订单真实性
            return IsPaid(strOrderNo, "") ? EnrollStatus.PaySuccess : EnrollStatus.PayFaild;
        }

        protected override PayModelDto GetPayModel()
        {
            Enroll enroll = _helper.GetEnroll();
            return WxPayApi.GetPayModel(enroll.OrderNo);
        }

        //查询订单
        protected override bool IsPaid(string transaction_id, string out_trade_no)
        {
            return WxPayApi.IsPaid(transaction_id, out_trade_no);
        }
    }
}