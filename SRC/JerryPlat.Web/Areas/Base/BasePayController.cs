using JerryPlat.BLL.CommonMagage;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Base
{
    public class BasePayController : AuthurizeBaseController<EnrollHelper>
    {
        public async Task<ActionResult> Return()
        {
            //SortedDictionary<string, string> sPara = GetRequestGet();
            PayModelDto model = GetPayModel();
            EnrollStatus enrollStatus = GetReturnEnrollStatus();
            await _helper.SetPayStatus(enrollStatus, model);
            return Redirect($"/Mob/Enroll/Process");
        }

        [HttpPost]
        public async Task Notify()
        {
            //SortedDictionary<string, string> sPara = GetRequestPost();
            PayModelDto model = GetPayModel();
            EnrollStatus enrollStatus = GetNoticeEnrollStatus();
            await _helper.SetPayStatus(enrollStatus, model);
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="id">Dt_Deposit.OrderNo</param>
        /// <returns></returns>
        public override ActionResult Index()
        {
            Enroll enroll = _helper.GetEnroll();

            if (_helper.IsPaid(enroll))//本地判断是否支付
            {
                return Faild("已支付报名费。");
            }

            if (IsPaid("", enroll.OrderNo))//第三方判断是否支付
            {
                PayModelDto model = GetPayModel();
                _helper.SetPayStatus(EnrollStatus.PaySuccess, model).GetAwaiter().GetResult();
                return Faild("已支付报名费。");
            }

            return SetPostRequest(enroll);
        }

        public async Task<ActionResult> Check(int id)
        {
            Enroll enroll = await _helper.GetEnroll(id);
            if (_helper.IsPaid(enroll))
            {
                return Success();
            }
            PayModelDto model = GetPayModel();
            EnrollStatus enrollStatus = GetReturnEnrollStatus(enroll.OrderNo);
            await _helper.SetPayStatus(enrollStatus, model);
            return Success();
        }

        protected virtual ActionResult SetPostRequest(Enroll enroll)
        {
            throw new Exception("This method must be override!");
        }

        protected virtual EnrollStatus GetEnrollStatus(SortedDictionary<string, string> sPara)
        {
            throw new Exception("This method must be override!");
        }

        protected virtual PayModelDto GetPayModel()
        {
            throw new Exception("This method must be override!");
        }

        protected virtual bool IsPaid(string transaction_id, string out_trade_no)
        {
            throw new Exception("This method must be override!");
        }

        protected virtual EnrollStatus GetReturnEnrollStatus(string strOrderNo = "")
        {
            throw new Exception("This method must be override!");
        }

        protected virtual EnrollStatus GetNoticeEnrollStatus(string strOrderNo = "")
        {
            throw new Exception("This method must be override!");
        }

        protected string GetString(string strKey)
        {
            string strValue = Request.QueryString[strKey];
            if (string.IsNullOrEmpty(strValue))
            {
                strValue = Request.Form[strKey];
            }
            return string.IsNullOrEmpty(strValue) ? "" : strValue;
        }

        /// <summary>
        /// 获取GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        private SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            coll = Request.QueryString;

            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 获取POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        private SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            coll = Request.Form;

            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }
    }
}