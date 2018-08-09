namespace JerryPlat.Models.Dto
{
    public class LocationDto
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public float? Latitude { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public float? Longitude { get; set; }

        /// <summary>
        /// 地理位置精度 : 推送
        /// </summary>
        public float? Precision { get; set; }

        /// <summary>
        /// 地理位置精度 ：主动获取
        /// </summary>
        public float? Accuracy { get; set; }
    }
}