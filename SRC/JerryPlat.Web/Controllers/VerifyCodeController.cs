using JerryPlat.Utils.Helpers;
using System.Web.Mvc;

namespace JerryPlat.Web.Controllers
{
    public class VerifyCodeController : Controller
    {
        // GET: VerifyCode
        public ActionResult Index()
        {
            string strVerifyCode = VerifyCodeHelper.CreateRandomCode(4);
            SessionHelper.VerifyCode.SetSession(strVerifyCode);
            return File(VerifyCodeHelper.CreateValidateGraphic(strVerifyCode), "image/Jpeg");
        }
    }
}