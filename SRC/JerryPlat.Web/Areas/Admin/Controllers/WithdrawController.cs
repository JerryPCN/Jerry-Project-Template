using JerryPlat.BLL.CommonMagage;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class WithdrawController : AuthurizeBaseController<WithdrawHelper>
    {
        [HttpPost]
        public ActionResult GetList(SearchModel search, PageParam pageParam)
        {
            PageData<WithdrawDto> pageData = _helper.GetPageList(search, o => o.ApplyTime, pageParam, false);
            return Success(pageData);
        }

        [HttpPost]
        public async Task<ActionResult> Save(Withdraw model)
        {
            bool result = await _helper.SaveAsync(model);
            return Success(result);
        }

        [HttpPost]
        public async Task<ActionResult> Approve(int id)
        {
            bool result = await _helper.Approve(id);
            if (result)
            {
                return Success();
            }
            return Faild("优币不够，无法进行提现。");
        }

        [HttpPost]
        public async Task<ActionResult> Discard(int id)
        {
            bool result = await _helper.Discard(id);
            if (result)
            {
                return Success();
            }
            return Faild("已提现，无法进行废弃。");
        }
    }
}