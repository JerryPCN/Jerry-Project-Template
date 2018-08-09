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
    public class MemberController : AuthurizeBaseController<MemberHelper>
    {
        public ActionResult Team()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetTeamList(PageParam pageParam)
        {
            PageData<TeamDto> pageData = _helper.GetTeamReportList(new SearchModel { Id = _Member.Id }, o => o.JoinTime, pageParam, false);
            return Success(pageData);
        }

        public async Task<ActionResult> Score()
        {
            Member member = await _helper.GetByIdAsync(_Member.Id);
            if (member.Score != _Member.Score)
            {
                SessionHelper.Mob.SetSession(member);
            }
            return View(member.Score);
        }

        [HttpPost]
        public ActionResult GetScoreList(PageParam pageParam)
        {
            PageData<ScoreHistory> pageData = _helper.GetScoreList(new SearchModel { Id = _Member.Id }, o => o.UpdateTime, pageParam, false);
            return Success(pageData);
        }

        [HttpPost]
        public async Task<ActionResult> Bind(BindDto model)
        {
            if (!SessionHelper.SMS.IsValid(model.Code))
            {
                return Faild("您输入的验证码不对，请核实后重新输入。");
            }

            return Success(await _helper.Bind(model));
        }
    }
}