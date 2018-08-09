using JerryPlat.BLL.CommonManage;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class QuestionTypeController : AuthurizeBaseController<QuestionTypeHelper>
    {
        [HttpPost]
        public ActionResult GetList(SearchModel search, PageParam pageParam)
        {
            PageData<QuestionTypeDto> pageData = _helper.GetPageList(search, o => o.OrderIndex, pageParam, true);
            return Success(pageData);
        }

        [HttpPost]
        public async Task<ActionResult> Save(QuestionTypeDto model)
        {
            bool result = await _helper.SaveAsync(model);
            return Success(result);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            bool result = await _helper.DeleteAsync(id);
            return Success(result);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteList(List<int> idList)
        {
            bool result = await _helper.DeleteListAsync(idList);
            return Success(result);
        }
    }
}