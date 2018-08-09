using JerryPlat.Models.Dto;
using JerryPlat.Web.Areas.Base;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Mob.Controllers
{
    public class UserController : AuthurizeBaseController
    {
        // GET: Mob/Enroll
        public ActionResult Index()
        {
            return View(new UserDto
            {
                ShareCode = _Member.ShareCode,
                NickName = _Member.NickName,
                Avatar = _Member.Avatar
            });
        }

        public ActionResult Bind()
        {
            return View();
        }
    }
}