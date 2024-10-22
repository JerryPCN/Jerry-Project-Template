﻿using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using System;
using System.Web;
using System.Web.Security;

namespace JerryPlat.Pay.WxPay
{
    public class JsApiPay
    {
        /// <summary>
        /// openid用于调用统一下单接口
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// access_token用于获取收货地址js函数入口参数
        /// </summary>
        public string access_token { get; set; }

        private JsApiConfig jsApiConfig { get; set; }

        /// <summary>
        /// 统一下单接口返回结果
        /// </summary>
        public WxPayData unifiedOrderResult { get; set; }

        public JsApiPay()
        {
            jsApiConfig = new JsApiConfig();
        }

        /**
        *
        * 网页授权获取用户基本信息的全部过程
        * 详情请参看网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
        * 第一步：利用url跳转获取code
        * 第二步：利用code去获取openid和access_token
        *
        */

        public void GetOpenidAndAccessToken()
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["code"]))
            {
                //获取code码，以获取openid和access_token
                string code = HttpContext.Current.Request.QueryString["code"];
                GetOpenidAndAccessTokenFromCode(code);
            }
            else
            {
                //构造网页授权获取code的URL
                string scheme = HttpContext.Current.Request.Url.Scheme;
                string host = HttpContext.Current.Request.Url.Host;
                string path = HttpContext.Current.Request.Path;
                string redirect_uri = HttpUtility.UrlEncode(scheme + "://" + host + path);
                WxPayData data = new WxPayData();
                data.SetValue("appid", jsApiConfig.AppId);
                data.SetValue("redirect_uri", redirect_uri);
                data.SetValue("response_type", "code");
                data.SetValue("scope", "snsapi_base");
                data.SetValue("state", "STATE" + "#wechat_redirect");
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();
                try
                {
                    //触发微信返回code码
                    HttpContext.Current.Response.Redirect(url);//Redirect函数会抛出ThreadAbortException异常，不用处理这个异常
                }
                catch (System.Threading.ThreadAbortException ex)
                {
                }
            }
        }

        /**
	    *
	    * 通过code换取网页授权access_token和openid的返回数据，正确时返回的JSON数据包如下：
	    * {
	    *  "access_token":"ACCESS_TOKEN",
	    *  "expires_in":7200,
	    *  "refresh_token":"REFRESH_TOKEN",
	    *  "openid":"OPENID",
	    *  "scope":"SCOPE",
	    *  "unionid": "o6_bmasdasdsad6_2sgVt7hMZOPfL"
	    * }
	    * 其中access_token可用于获取共享收货地址
	    * openid是微信支付jsapi支付接口统一下单时必须的参数
        * 更详细的说明请参考网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
        * @失败时抛异常WxPayException
	    */

        public void GetOpenidAndAccessTokenFromCode(string code)
        {
            try
            {
                //构造获取openid及access_token的url
                WxPayData data = new WxPayData();
                data.SetValue("appid", jsApiConfig.AppId);
                data.SetValue("secret", jsApiConfig.AppSecret);
                data.SetValue("code", code);
                data.SetValue("grant_type", "authorization_code");
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?" + data.ToUrl();

                //请求url以获取数据
                string result = HttpService.Get(url);

                OwinTokenDto model = SerializationHelper.JsonToObject<OwinTokenDto>(result);

                //保存access_token，用于收货地址获取
                access_token = model.Access_Token;

                //获取用户openid
                openid = model.OpenId;
            }
            catch (Exception ex)
            {
                throw new WxPayException(ex.ToString());
            }
        }

        /**
         * 调用统一下单，获得下单结果
         * @return 统一下单结果
         * @失败时抛异常WxPayException
         */

        public WxPayData GetUnifiedOrderResult(Enroll enroll, Course course)
        {
            //统一下单
            WxPayData data = new WxPayData();
            data.SetValue("body", "优杰学车-在线报名");
            data.SetValue("detail", $"客户于{enroll.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")}在线报名{course.Name}{enroll.Amount}元."); //商品详情
            data.SetValue("out_trade_no", enroll.OrderNo);
            data.SetValue("total_fee", (Math.Round(enroll.Amount * 100, 0)).ToString());//微信支付提交的金额是不能带小数点的，且是以分为单位，所以我们系统如果是以元为单位要处理下金额，即先乘以100，再去小数点
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", openid);

            WxPayData result = WxPayApi.UnifiedOrder(data);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                throw new WxPayException("UnifiedOrder response error!");
            }

            unifiedOrderResult = result;
            return result;
        }

        /**
        *
        * 从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
        * 微信浏览器调起JSAPI时的输入参数格式如下：
        * {
        *   "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入
        *   "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数
        *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串
        *   "package" : "prepay_id=u802345jgfjsdfgsdg888",
        *   "signType" : "MD5",         //微信签名方式:
        *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名
        * }
        * @return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
        * 更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
        *
        */

        public WxPayData GetJsApiParameters()
        {
            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", unifiedOrderResult.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + unifiedOrderResult.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

            return jsApiParam;
        }

        /**
	    *
	    * 获取收货地址js函数入口参数,详情请参考收货地址共享接口：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_9
	    * @return string 共享收货地址js函数需要的参数，json格式可以直接做参数使用
	    */

        public string GetEditAddressParameters()
        {
            string parameter = "";
            try
            {
                string host = HttpContext.Current.Request.Url.Host;
                string path = HttpContext.Current.Request.Path;
                string queryString = HttpContext.Current.Request.Url.Query;
                //这个地方要注意，参与签名的是网页授权获取用户信息时微信后台回传的完整url
                string url = "http://" + host + path + queryString;

                //构造需要用SHA1算法加密的数据
                WxPayData signData = new WxPayData();
                signData.SetValue("appid", jsApiConfig.AppId);
                signData.SetValue("url", url);
                signData.SetValue("timestamp", WxPayApi.GenerateTimeStamp());
                signData.SetValue("noncestr", WxPayApi.GenerateNonceStr());
                signData.SetValue("accesstoken", access_token);
                string param = signData.ToUrl();

                //SHA1加密
                string addrSign = FormsAuthentication.HashPasswordForStoringInConfigFile(param, "SHA1");

                //获取收货地址js函数入口参数
                WxPayData afterData = new WxPayData();
                afterData.SetValue("appId", jsApiConfig.AppId);
                afterData.SetValue("scope", "jsapi_address");
                afterData.SetValue("signType", "sha1");
                afterData.SetValue("addrSign", addrSign);
                afterData.SetValue("timeStamp", signData.GetValue("timestamp"));
                afterData.SetValue("nonceStr", signData.GetValue("noncestr"));

                //转为json格式
                parameter = afterData.ToJson();
            }
            catch (Exception ex)
            {
                throw new WxPayException(ex.ToString());
            }

            return parameter;
        }
    }
}