using System.Web;
using System.Xml;

namespace JerryPlat.Pay.WxPay
{
    public class JsApiConfig
    {
        #region 字段

        private string partner = string.Empty;
        private string key = string.Empty;
        private string appid = string.Empty;
        private string appsecret = string.Empty;
        private string redirect_url = string.Empty;
        private string notify_url = string.Empty;

        #endregion 字段

        public JsApiConfig()
        {
            //读取XML配置信息
            string fullPath = HttpContext.Current.Server.MapPath("~/xmlconfig/wxapipay.config");
            XmlDocument doc = new XmlDocument();
            doc.Load(fullPath);
            XmlNode _partner = doc.SelectSingleNode(@"Root/partner");
            XmlNode _key = doc.SelectSingleNode(@"Root/key");
            XmlNode _appid = doc.SelectSingleNode(@"Root/appid");
            XmlNode _appsecret = doc.SelectSingleNode(@"Root/appsecret");
            XmlNode _redirect_url = doc.SelectSingleNode(@"Root/redirect_url");
            XmlNode _notify_url = doc.SelectSingleNode(@"Root/notify_url");
            //读取站点配置信息 并缓存
            //Model.siteconfig model = new BLL.siteconfig().loadConfig();

            //商户号（必须配置）
            partner = _partner.InnerText;
            //商户支付密钥，参考开户邮件设置（必须配置）
            key = _key.InnerText;
            //绑定支付的APPID（必须配置）
            appid = _appid.InnerText;
            //公众帐号secert（仅JSAPI支付的时候需要配置）
            appsecret = _appsecret.InnerText;
            //获取用户的OPENID回调地址
            redirect_url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority.ToLower() + _redirect_url.InnerText;
            //回调处理地址
            notify_url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority.ToLower() + _notify_url.InnerText;
        }

        #region 属性

        /// <summary>
        /// 商户号（必须配置）
        /// </summary>
        public string Partner
        {
            get { return partner; }
            set { partner = value; }
        }

        /// <summary>
        /// 获取或设交易安全校验码
        /// </summary>
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// 绑定支付的APPID（必须配置）
        /// </summary>
        public string AppId
        {
            get { return appid; }
            set { appid = value; }
        }

        /// <summary>
        /// 公众帐号secert（仅JSAPI支付的时候需要配置）
        /// </summary>
        public string AppSecret
        {
            get { return appsecret; }
            set { appsecret = value; }
        }

        /// <summary>
        /// 获取用户的OPENID回调地址
        /// </summary>
        public string Redirect_url
        {
            get { return redirect_url; }
        }

        /// <summary>
        /// 获取服务器异步通知页面路径
        /// </summary>
        public string Notify_url
        {
            get { return notify_url; }
        }

        #endregion 属性
    }
}