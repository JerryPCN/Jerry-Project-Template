using JerryPlat.Utils.Models;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace JerryPlat.Utils.Helpers
{
    public class SMSReturnStatus
    {
        public string returnstatus { get; set; }
        public string message { get; set; }
        public string remainpoint { get; set; }
        public string taskID { get; set; }
        public string successCounts { get; set; }
    }

    public class SMS
    {
        //短信信息，后台 http://c.ipyy.net:8080/
        private readonly string smsapiurl = SystemConfigModel.Instance.SmsGateway;// "http://sh2.cshxsp.com/smsJson.aspx";

        private readonly string smsusername = SystemConfigModel.Instance.SmsAccount;//"AA00424sjwx";
        private readonly string smspassword = SystemConfigModel.Instance.SmsPassword;//"AA00424sjwx."; //"qwe159";

        //private readonly string epid = "121717";
        private readonly string strQianMing = SystemConfigModel.Instance.SmsSignature;//"【蜂巢】";

        public SMS()
        {
        }

        /// <summary>
        /// 发送手机短信
        /// </summary>
        /// <param name="mobiles">手机号码，以英文“,”逗号分隔开</param>
        /// <param name="content">短信内容</param>
        /// <param name="pass">短信通道1验证码通道2广告通道</param>
        /// <param name="msg">返回提示信息</param>
        /// <returns>bool</returns>
        public bool Send(string mobiles, string content, out string msg)
        {
            //检查手机号码，如果超过2000则分批发送
            int sucCount = 0; //成功提交数量
            string errorMsg = string.Empty; //错误消息
            string[] oldMobileArr = mobiles.Split(',');
            int batch = oldMobileArr.Length / 50 + 1; //2000条为一批，求出分多少批

            for (int i = 0; i < batch; i++)
            {
                StringBuilder sb = new StringBuilder();
                int sendCount = 0; //发送数量
                int maxLenght = (i + 1) * 50; //循环最大的数

                //检测号码，忽略不合格的，重新组合
                for (int j = 0; j < oldMobileArr.Length && j < maxLenght; j++)
                {
                    int arrNum = j + (i * 50);
                    string pattern = @"^1\d{10}$";
                    string mobile = oldMobileArr[arrNum].Trim();
                    Regex r = new Regex(pattern, RegexOptions.IgnoreCase); //正则表达式实例，不区分大小写
                    Match m = r.Match(mobile); //搜索匹配项
                    if (m != null)
                    {
                        sendCount++;
                        sb.Append(mobile + ",");
                    }
                }

                //发送短信
                if (sb.ToString().Length > 0)
                {
                    try
                    {
                        string strSmsContent = strQianMing + content;
                        //LogHelper.Info(strSmsContent);
                        //strSmsContent = UrlEncode(strSmsContent);
                        string strParams = "action=send&userid=&account=" + smsusername + "&password=" + smspassword + "&mobile=" + DelLastComma(sb.ToString()) + "&content=" + strSmsContent + "&sendTime=&extno=";
                        //_Log.Info(strParams);
                        string result = HttpPost(smsapiurl, strParams, ref errorMsg);
                        //_Log.Info(result);
                        if (result == null)
                        {
                            msg = errorMsg;
                            return false;
                        }

                        SMSReturnStatus smsReturnStatus = null;
                        try
                        {
                            smsReturnStatus = SerializationHelper.JsonToObject<SMSReturnStatus>(result);
                            if (smsReturnStatus.returnstatus == "Success")
                            {
                                sucCount += sendCount;
                            }
                            else
                            {
                                errorMsg = smsReturnStatus.message;
                                LogHelper.Error(errorMsg);
                            }
                        }
                        catch (Exception ex)
                        {
                            msg = result;
                            return false;
                        }
                        //成功数量
                    }
                    catch (Exception ex)
                    {
                        errorMsg = ex.Message + ex.StackTrace;
                        LogHelper.Error("Faild", ex);
                        //没有动作
                    }
                }
            }

            //返回状态
            if (sucCount > 0)
            {
                msg = "成功提交" + sucCount + "条，失败" + (oldMobileArr.Length - sucCount) + "条";
                return true;
            }
            msg = errorMsg;
            return false;
        }

        /// <summary>
        /// 查询账户剩余短信数量
        /// </summary>
        //public int GetAccountQuantity()
        //{
        //    try
        //    {
        //        string result = HttpPost(smsapiurl + "getfee/", "userid=&username=" + smsusername + "&password=" + smspassword + "&epid=" + epid);
        //        return int.Parse(result);
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //}

        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param, ref string errorMsg)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message + ex.StackTrace;
                LogHelper.Error(ex);
                throw ex;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }

        #region 删除最后结尾的一个逗号

        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            if (str.Length < 1)
            {
                return "";
            }
            return str.Substring(0, str.LastIndexOf(","));
        }

        #endregion 删除最后结尾的一个逗号

        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            str = str.Replace("'", "");
            return HttpUtility.UrlEncode(str, Encoding.UTF8);
        }
    }
}