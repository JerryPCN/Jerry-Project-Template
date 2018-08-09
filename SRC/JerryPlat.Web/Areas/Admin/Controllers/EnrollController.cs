using JerryPlat.BLL.CommonMagage;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Office;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class EnrollController : AuthurizeBaseController<EnrollHelper>
    {
        [HttpPost]
        public ActionResult GetList(SearchModel search, PageParam pageParam)
        {
            PageData<EnrollDto> pageData = _helper.GetPageList(search, o => o.UpdateTime, pageParam, false);
            return Success(pageData);
        }

        public ActionResult Export(SearchModel search)
        {
            List<EnrollExportDto> enrollExportList = _helper.GetPageList(search, o => o.UpdateTime, false);
            return File(ExcelHelper.SaveExcelContent(enrollExportList)
                        , ExcelHelper.Excel_ContentType
                        , ExcelHelper.GetExcelName("EnrollExport"));
        }

        [HttpPost]
        public async Task<ActionResult> Approve(Enroll enroll)
        {
            await _helper.Approve(enroll);
            return Success();
        }

        [HttpPost]
        public async Task<ActionResult> Reverse(Enroll enroll)
        {
            if (!_helper.IsPaid(enroll))
            {
                return Faild("当前报名还未支付，不能进行撤销收款。");
            }

            if (await _helper.IsAutoScore(enroll))
            {
                return Faild("当前报名所在月已经进么了自动优币奖励，不能进行撤销收款。");
            }

            await _helper.Reverse(enroll);
            return Success();
        }

        [HttpPost]
        public async Task<ActionResult> Save(EnrollDto model)
        {
            bool result = await _helper.SaveAsync(model.Enroll);
            return Success(result);
        }
    }
}