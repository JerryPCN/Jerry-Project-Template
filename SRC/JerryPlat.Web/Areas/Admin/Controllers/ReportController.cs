using JerryPlat.BLL.CommonMagage;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class ReportController : AuthurizeBaseController<PayRecordHelper>
    {
        [HttpPost]
        public ActionResult GetPayRecordList(SearchModel search, PageParam pageParam)
        {
            PageData<PayRecordDto> pageData = _helper.GetPageList(search, o => o.UpdateTime, pageParam, false);
            return Success(pageData);
        }
    }
}