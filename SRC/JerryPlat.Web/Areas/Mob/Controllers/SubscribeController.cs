using JerryPlat.BLL.CommonMagage;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Mob.Controllers
{
    public class SubscribeController : AuthurizeBaseController<SubscribeHelper>
    {
        // GET: Mob/Enroll
        public override ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Save(string code, Subscribe model)
        {
            if (!SessionHelper.VerifyCode.IsValid(code))
            {
                return Faild("请输入正确的验证码。");
            }
            return Success(await _helper.SaveAsync(model));
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetList(SearchModel seachModel, PageParam pageParam)
        {
            PageData<SubscribeDto> pageData = _helper.GetPageList(seachModel, o => o.TeachTime, pageParam, false, true);
            return Success(pageData);
        }
    }
}