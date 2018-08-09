using JerryPlat.Utils.Helpers;
using System.Collections.Generic;

namespace JerryPlat.Utils.Models
{
    public class SystemConfigModel
    {
        public static SystemConfigModel Instance;

        public static void Reset(Dictionary<string, string> keyValueList)
        {
            SystemConfigModel.Instance = TypeHelper.InitModel<SystemConfigModel>(keyValueList);
        }

        public static void Reset(SystemConfigModel model)
        {
            SystemConfigModel.Instance = model;
        }

        #region 基础设置

        public string DefaultPassword { get; set; }
        public bool IsUseVerifyCode { get; set; }
        public bool IsUseSms { get; set; }
        public decimal TaxPercentage { get; set; }

        #endregion 基础设置

        #region 移动端首页设置

        public string TopLeftName { get; set; }
        public string TopLeftUrl { get; set; }
        public string TopRightName { get; set; }
        public string TopRightUrl { get; set; }
        public string BottomLeftName { get; set; }
        public string BottomLeftUrl { get; set; }
        public string BottomRightName { get; set; }
        public string BottomRightUrl { get; set; }

        #endregion 移动端首页设置

        #region 优币设置 A---->B---->C

        #region Rule 1: C报名支付后，B获取500， A获取200

        public int RefereeScore { get; set; }
        public string RefereeScoreDescription { get; set; }
        public int ParentRefereeScore { get; set; }
        public string ParentRefereeScoreDescription { get; set; }

        #endregion Rule 1: C报名支付后，B获取500， A获取200

        #region Rule 2: 当月(B1,B2…) >=10人，A获取每人100，包括前10人  (C1,C2…) >=30人，A获取每人50，包括前30人

        public int FirstCount { get; set; }
        public int FirstScore { get; set; }
        public string FirstDescription { get; set; }
        public int SecondCount { get; set; }
        public int SecondScore { get; set; }
        public string SecondDescription { get; set; }

        #endregion Rule 2: 当月(B1,B2…) >=10人，A获取每人100，包括前10人  (C1,C2…) >=30人，A获取每人50，包括前30人

        #region Rule 3: 当月(B1,B2…) >=10人，A才有资格参与比赛，参与比赛第三名可拿下所有当月B的人数*50

        public int MatchCount { get; set; }
        public int MatchScore { get; set; }
        public string MatchDescription { get; set; }
        public string GradePercentage { get; set; }

        #endregion Rule 3: 当月(B1,B2…) >=10人，A才有资格参与比赛，参与比赛第三名可拿下所有当月B的人数*50

        #endregion 优币设置 A---->B---->C

        #region 分享设置

        public string ShareTitle { get; set; }
        public string ShareContent { get; set; }

        #endregion 分享设置

        #region 短信设置

        public string SmsGateway { get; set; }
        public string SmsAccount { get; set; }
        public string SmsPassword { get; set; }
        public string SmsSignature { get; set; }
        public string SmsCodeTemplate { get; set; }
        public string SmsPayTemplate { get; set; }

        #endregion 短信设置
    }
}