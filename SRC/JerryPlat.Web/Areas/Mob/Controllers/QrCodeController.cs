using JerryPlat.BLL.CommonMagage;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using JerryPlat.Web.Areas.Base;
using System.Drawing;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Mob.Controllers
{
    public class QrCodeController : BaseController
    {
        // GET: Mob/Home
        /// <summary>
        ///
        /// </summary>
        /// <param name="id">ShareCode</param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string id, int size = 9)
        {
            MemberHelper helper = new MemberHelper();
            Member member = await helper.GetByShareCodeAsync(id);
            string strIconPath = member == null ? "" : member.Avatar;

            string strContent = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + "/Mob/Home/Index/" + id;

            Bitmap bitMap = QrCodeHelper.Create(strContent, size, strIconPath);

            return File(TypeHelper.Bitmap2Byte(bitMap), "image/Jpeg");
        }
    }
}