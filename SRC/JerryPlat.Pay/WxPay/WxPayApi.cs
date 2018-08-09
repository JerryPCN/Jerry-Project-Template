using JerryPlat.BLL.CommonMagage;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using LitJson;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JerryPlat.Pay.WxPay
{
    public class WxPayApi
    {
        public static PayModelDto GetPayModel(string strOrderNo)
        {
            return new PayModelDto
            {
                OrderNo = strOrderNo,
                PayType = "微信支付"
            };
        }

        public static bool IsPaid(string transaction_id, string out_trade_no, int timeOut = 6)
        {
            WxPayData res = WxPayApi.OrderQuery(transaction_id, out_trade_no, timeOut);
            return res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS";
        }

        public static WxPayData OrderQuery(string transaction_id, string out_trade_no, int timeOut = 6)
        {
            WxPayData inputObj = new WxPayData();
            inputObj.SetValue("transaction_id", transaction_id);
            inputObj.SetValue("out_trade_no", out_trade_no);
            return OrderQuery(inputObj, timeOut);
        }

        /**
        *
        * 查询订单
        * @param WxPayData inputObj 提交给查询订单API的参数
        * @param int timeOut 超时时间
        * @throws WxPayException
        * @return 成功时返回订单查询结果，其他抛异常
        */

        public static WxPayData OrderQuery(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("订单查询接口中，out_trade_no、transaction_id至少填一个！");
            }

            JsApiConfig jsApiConfig = new JsApiConfig();

            inputObj.SetValue("appid", jsApiConfig.AppId);//公众账号ID
            inputObj.SetValue("mch_id", jsApiConfig.Partner);//商户号
            inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();

            var start = DateTime.Now;

            string response = HttpService.Post(xml, url, false, timeOut);//调用HTTP通信接口提交数据

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的数据转化为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }

        /**
        *
        * 撤销订单API接口
        * @param WxPayData inputObj 提交给撤销订单API接口的参数，out_trade_no和transaction_id必填一个
        * @param int timeOut 接口超时时间
        * @throws WxPayException
        * @return 成功时返回API调用结果，其他抛异常
        */

        public static WxPayData Reverse(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/reverse";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("撤销订单API接口中，参数out_trade_no和transaction_id必须填写一个！");
            }

            JsApiConfig jsApiConfig = new JsApiConfig();

            inputObj.SetValue("appid", jsApiConfig.AppId);//公众账号ID
            inputObj.SetValue("mch_id", jsApiConfig.Partner);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            string response = HttpService.Post(xml, url, true, timeOut);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }

        /**
        *
        * 申请退款
        * @param WxPayData inputObj 提交给申请退款API的参数
        * @param int timeOut 超时时间
        * @throws WxPayException
        * @return 成功时返回接口调用结果，其他抛异常
        */

        public static WxPayData Refund(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("退款申请接口中，out_trade_no、transaction_id至少填一个！");
            }
            else if (!inputObj.IsSet("out_refund_no"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数out_refund_no！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数total_fee！");
            }
            else if (!inputObj.IsSet("refund_fee"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数refund_fee！");
            }
            else if (!inputObj.IsSet("op_user_id"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数op_user_id！");
            }

            JsApiConfig jsApiConfig = new JsApiConfig();

            inputObj.SetValue("appid", jsApiConfig.AppId);//公众账号ID
            inputObj.SetValue("mch_id", jsApiConfig.Partner);//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();
            var start = DateTime.Now;

            string response = HttpService.Post(xml, url, true, timeOut);//调用HTTP通信接口提交数据到API

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }

        /**
	    *
	    * 查询退款
	    * 提交退款申请后，通过该接口查询退款状态。退款有一定延时，
	    * 用零钱支付的退款20分钟内到账，银行卡支付的退款3个工作日后重新查询退款状态。
	    * out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个
	    * @param WxPayData inputObj 提交给查询退款API的参数
	    * @param int timeOut 接口超时时间
	    * @throws WxPayException
	    * @return 成功时返回，其他抛异常
	    */

        public static WxPayData RefundQuery(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/refundquery";
            //检测必填参数
            if (!inputObj.IsSet("out_refund_no") && !inputObj.IsSet("out_trade_no") &&
                !inputObj.IsSet("transaction_id") && !inputObj.IsSet("refund_id"))
            {
                throw new WxPayException("退款查询接口中，out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个！");
            }

            JsApiConfig jsApiConfig = new JsApiConfig();

            inputObj.SetValue("appid", jsApiConfig.AppId);//公众账号ID
            inputObj.SetValue("mch_id", jsApiConfig.Partner);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            string response = HttpService.Post(xml, url, false, timeOut);//调用HTTP通信接口以提交数据到API

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }

        /**
        * 下载对账单
        * @param WxPayData inputObj 提交给下载对账单API的参数
        * @param int timeOut 接口超时时间
        * @throws WxPayException
        * @return 成功时返回，其他抛异常
        */

        public static WxPayData DownloadBill(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/downloadbill";
            //检测必填参数
            if (!inputObj.IsSet("bill_date"))
            {
                throw new WxPayException("对账单接口中，缺少必填参数bill_date！");
            }

            JsApiConfig jsApiConfig = new JsApiConfig();

            inputObj.SetValue("appid", jsApiConfig.AppId);//公众账号ID
            inputObj.SetValue("mch_id", jsApiConfig.Partner);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();

            string response = HttpService.Post(xml, url, false, timeOut);//调用HTTP通信接口以提交数据到API

            WxPayData result = new WxPayData();
            //若接口调用失败会返回xml格式的结果
            if (response.Substring(0, 5) == "<xml>")
            {
                result.FromXml(response);
            }
            //接口调用成功则返回非xml格式的数据
            else
                result.SetValue("result", response);

            return result;
        }

        /**
	    *
	    * 转换短链接
	    * 该接口主要用于扫码原生支付模式一中的二维码链接转成短链接(weixin://wxpay/s/XXXXXX)，
	    * 减小二维码数据量，提升扫描速度和精确度。
	    * @param WxPayData inputObj 提交给转换短连接API的参数
	    * @param int timeOut 接口超时时间
	    * @throws WxPayException
	    * @return 成功时返回，其他抛异常
	    */

        public static WxPayData ShortUrl(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/tools/shorturl";
            //检测必填参数
            if (!inputObj.IsSet("long_url"))
            {
                throw new WxPayException("需要转换的URL，签名用原串，传输需URL encode！");
            }

            JsApiConfig jsApiConfig = new JsApiConfig();
            inputObj.SetValue("appid", jsApiConfig.AppId);//公众账号ID
            inputObj.SetValue("mch_id", jsApiConfig.Partner);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            string response = HttpService.Post(xml, url, false, timeOut);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);
            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }

        /**
        *
        * 统一下单
        * @param WxPaydata inputObj 提交给统一下单API的参数
        * @param int timeOut 超时时间
        * @throws WxPayException
        * @return 成功时返回，其他抛异常
        */

        public static WxPayData UnifiedOrder(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new WxPayException("缺少统一支付接口必填参数out_trade_no！");
            }
            else if (!inputObj.IsSet("body"))
            {
                throw new WxPayException("缺少统一支付接口必填参数body！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new WxPayException("缺少统一支付接口必填参数total_fee！");
            }
            else if (!inputObj.IsSet("trade_type"))
            {
                throw new WxPayException("缺少统一支付接口必填参数trade_type！");
            }

            //关联参数
            if (inputObj.GetValue("trade_type").ToString() == "JSAPI" && !inputObj.IsSet("openid"))
            {
                throw new WxPayException("统一支付接口中，缺少必填参数openid！trade_type为JSAPI时，openid为必填参数！");
            }
            if (inputObj.GetValue("trade_type").ToString() == "NATIVE" && !inputObj.IsSet("product_id"))
            {
                throw new WxPayException("统一支付接口中，缺少必填参数product_id！trade_type为JSAPI时，product_id为必填参数！");
            }

            JsApiConfig jsApiConfig = new JsApiConfig();

            //异步通知url未设置，则使用配置文件中的url
            if (!inputObj.IsSet("notify_url"))
            {
                inputObj.SetValue("notify_url", jsApiConfig.Notify_url);//异步通知url
            }

            inputObj.SetValue("appid", jsApiConfig.AppId);//公众账号ID
            inputObj.SetValue("mch_id", jsApiConfig.Partner);//商户号
            inputObj.SetValue("spbill_create_ip", ToolHelper.GetIP());//终端ip
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串

            //签名
            inputObj.SetValue("sign", inputObj.MakeSign());
            string xml = inputObj.ToXml();

            var start = DateTime.Now;

            string response = HttpService.Post(xml, url, false, timeOut);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }

        /**
	    *
	    * 关闭订单
	    * @param WxPayData inputObj 提交给关闭订单API的参数
	    * @param int timeOut 接口超时时间
	    * @throws WxPayException
	    * @return 成功时返回，其他抛异常
	    */

        public static WxPayData CloseOrder(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/closeorder";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new WxPayException("关闭订单接口中，out_trade_no必填！");
            }

            JsApiConfig jsApiConfig = new JsApiConfig();

            inputObj.SetValue("appid", jsApiConfig.AppId);//公众账号ID
            inputObj.SetValue("mch_id", jsApiConfig.Partner);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            string response = HttpService.Post(xml, url, false, timeOut);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }

        /**
	    *
	    * 测速上报
	    * @param string interface_url 接口URL
	    * @param int timeCost 接口耗时
	    * @param WxPayData inputObj参数数组
	    */

        private static void ReportCostTime(string interface_url, int timeCost, WxPayData inputObj)
        {
            return;
        }

        /**
	    *
	    * 测速上报接口实现
	    * @param WxPayData inputObj 提交给测速上报接口的参数
	    * @param int timeOut 测速上报接口超时时间
	    * @throws WxPayException
	    * @return 成功时返回测速上报接口返回的结果，其他抛异常
	    */

        public static WxPayData Report(WxPayData inputObj, int timeOut = 1)
        {
            string url = "https://api.mch.weixin.qq.com/payitil/report";
            //检测必填参数
            if (!inputObj.IsSet("interface_url"))
            {
                throw new WxPayException("接口URL，缺少必填参数interface_url！");
            }
            if (!inputObj.IsSet("return_code"))
            {
                throw new WxPayException("返回状态码，缺少必填参数return_code！");
            }
            if (!inputObj.IsSet("result_code"))
            {
                throw new WxPayException("业务结果，缺少必填参数result_code！");
            }
            if (!inputObj.IsSet("user_ip"))
            {
                throw new WxPayException("访问接口IP，缺少必填参数user_ip！");
            }
            if (!inputObj.IsSet("execute_time_"))
            {
                throw new WxPayException("接口耗时，缺少必填参数execute_time_！");
            }

            JsApiConfig jsApiConfig = new JsApiConfig();

            inputObj.SetValue("appid", jsApiConfig.AppId);//公众账号ID
            inputObj.SetValue("mch_id", jsApiConfig.Partner);//商户号
            inputObj.SetValue("user_ip", ToolHelper.GetIP());//终端ip
            inputObj.SetValue("time", DateTime.Now.ToString("yyyyMMddHHmmss"));//商户上报时间
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            string response = HttpService.Post(xml, url, false, timeOut);

            WxPayData result = new WxPayData();
            result.FromXml(response);
            return result;
        }

        /**
        * 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
         * @return 时间戳
        */

        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /**
        * 生成随机串，随机串包含字母或数字
        * @return 随机串
        */

        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public static WxPayData GetNotifyData()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = HttpContext.Current.Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            JsApiConfig jsApiConfig = new JsApiConfig();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                HttpContext.Current.Response.Write(res.ToXml());
                HttpContext.Current.Response.End();
            }

            return data;
        }

        public static async Task<WxPayData> GetJSSDKConfig()
        {
            JsApiConfig jsApiConfig = new JsApiConfig();

            string appid = jsApiConfig.AppId;

            string secret = jsApiConfig.AppSecret;

            string timestamp = WxPayApi.GenerateTimeStamp();

            string noncestr = WxPayApi.GenerateNonceStr();

            string signature = "";

            //签名算法  https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141115

            //1. 获取AccessToken（有效期7200秒，开发者必须在自己的服务全局缓存access_token）

            WxTicketHelper helper = new WxTicketHelper();

            WxTicket wxticket = await helper.GetWxTicketAsync(WxTicketType.ACCESS_TOKEN);
            string access_token = string.Empty;
            if (wxticket == null || wxticket.ExpireTime < ConvertDateTimeInt(DateTime.Now))
            {
                string url1 = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;

                string result = HttpService.Get(url1);

                JsonData jd = JsonMapper.ToObject(result);
                if (!((System.Collections.IDictionary)jd).Contains("access_token"))
                {
                    LogHelper.Error("access_token is null：" + result);
                    return null;
                }
                access_token = (string)jd["access_token"];

                await helper.SaveAsync(WxTicketType.ACCESS_TOKEN, new WxTicket
                {
                    Ticket = access_token,
                    ExpireTime = ConvertDateTimeInt(DateTime.Now) + 7180
                });
            }
            else
            {
                access_token = wxticket.Ticket;
            }

            //2. 用第一步拿到的access_token 采用http GET方式请求获得jsapi_ticket（有效期7200秒，开发者必须在自己的服务全局缓存jsapi_ticket）
            wxticket = await helper.GetWxTicketAsync(WxTicketType.TICKET);
            string ticket = string.Empty;
            if (wxticket == null || wxticket.ExpireTime < ConvertDateTimeInt(DateTime.Now))
            {
                string url2 = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + access_token + "&type=jsapi";

                string result2 = HttpService.Get(url2);

                JsonData jd2 = JsonMapper.ToObject(result2);

                if (!((System.Collections.IDictionary)jd2).Contains("ticket"))
                {
                    return null;
                }

                ticket = (string)jd2["ticket"];

                await helper.SaveAsync(WxTicketType.TICKET, new WxTicket
                {
                    Ticket = ticket,
                    ExpireTime = ConvertDateTimeInt(DateTime.Now) + 7180
                });
            }
            else
            {
                ticket = wxticket.Ticket;
            }

            //3. 开始签名

            Uri uri = HttpContext.Current.Request.Url;
            string now_url = (uri.Scheme + "://" + uri.Host + HttpContext.Current.Request.RawUrl).Split('#')[0];

            string no_jiami = $"jsapi_ticket={ticket}&noncestr={noncestr}&timestamp={timestamp}&url={now_url}";

            //SHA1加密

            signature = SHA1_Hash(no_jiami);

            WxPayData data = new WxPayData();

            data.SetValue("appId", appid);

            data.SetValue("timestamp", timestamp);

            data.SetValue("nonceStr", noncestr);

            data.SetValue("signature", signature);

            return data;
        }

        //SHA1哈希加密算法
        public static string SHA1_Hash(string str_sha1_in)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = System.Text.UTF8Encoding.Default.GetBytes(str_sha1_in);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.Replace("-", "").ToLower();
            return str_sha1_out;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            int intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = Convert.ToInt32((time - startTime).TotalSeconds);
            return intResult;
        }
    }
}