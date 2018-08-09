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
    public class WithdrawController : AuthurizeBaseController<WithdrawHelper>
    {
        // GET: Mob/Withdraw
        public override ActionResult Index()
        {
            //if (DateTime.Now.DayOfWeek != DayOfWeek.Friday)
            //{
            //    return Redirect("/Mob/Member/Score");
            //}

            MemberHelper helper = new MemberHelper();
            Member member = helper.GetById(_Member.Id);
            if (member.Score != _Member.Score)
            {
                SessionHelper.Mob.SetSession(member);
            }
            return View(member);
        }

        [HttpPost]
        public async Task<ActionResult> Save(Withdraw model)
        {
            model.MemberId = _Member.Id;
            model.TaxPercentage = SystemConfigModel.Instance.TaxPercentage;
            return Success(await _helper.SaveAsync(model));
        }

        public ActionResult Record()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetWithdrawScore()
        {
            return Success(_helper.GetWithdrawScore(true));
        }

        [HttpPost]
        public ActionResult GetList(PageParam pageParam)
        {
            return Success(_helper.GetPageList(new SearchModel(), o => o.OperateTime, pageParam, false, true));
        }
    }
}