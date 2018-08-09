using JerryPlat.Utils.Helpers;
using JerryPlat.Web.Areas.Base;
using System.Web.Mvc;

namespace JerryPlat.Web.Controllers
{
    public class SmsController : BaseController
    {
        /// <summary>
        /// SendCode
        /// </summary>
        /// <param name="id">Phone</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendCode(string id)
        {
            string strMsg = string.Empty;
            if (SMSHelper.SendCode(id, out strMsg))
            {
                return Success();
            }
            return Faild(strMsg);
        }
    }
}