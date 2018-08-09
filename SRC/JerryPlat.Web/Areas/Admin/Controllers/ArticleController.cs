using JerryPlat.BLL.CommonMagage;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class ArticleController : AuthurizeBaseController<ArticleHelper>
    {
        [HttpPost]
        public ActionResult GetList(SearchModel search, PageParam pageParam)
        {
            PageData<ArticleDto> pageData = _helper.GetPageList(search, o => o.UpdateTime, pageParam, false);
            return Success(pageData);
        }

        [HttpPost]
        public async Task<ActionResult> GetById(int id)
        {
            return Success(await _helper.GetByIdAsync(id));
        }

        [HttpPost]
        //[ValidateInput(false)]
        public async Task<ActionResult> Save(ArticleDto model)
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