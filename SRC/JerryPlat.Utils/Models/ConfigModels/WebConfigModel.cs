using JerryPlat.Utils.Helpers;

namespace JerryPlat.Utils.Models
{
    public class WebConfigModel
    {
        public static WebConfigModel Instance = TypeHelper.InitModel<WebConfigModel>(ConfigHelper.GetConfig);

        public string Owin_ClientId { get; set; }
        public string Owin_ClientSecret { get; set; }
        public string Api_BaseUrl { get; set; }
    }
}