using JerryPlat.BLL.CommonMagage;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Office;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class MemberController : AuthurizeBaseController<MemberHelper>
    {
        [HttpPost]
        public ActionResult GetList(SearchModel search, PageParam pageParam)
        {
            PageData<Member> pageData = _helper.GetTeamList(search, o => o.JoinTime, pageParam, false);
            return Success(pageData);
        }

        [HttpPost]
        public ActionResult GetScoreList(SearchModel search, PageParam pageParam)
        {
            PageData<ScoreHistory> pageData = _helper.GetScoreList(search, o => o.UpdateTime, pageParam, false);
            return Success(pageData);
        }

        public ActionResult Export(SearchModel search)
        {
            List<Member> memberList = _helper.GetTeamList(search, o => o.JoinTime, false);
            return File(ExcelHelper.SaveExcelContent(memberList)
                        , ExcelHelper.Excel_ContentType
                        , ExcelHelper.GetExcelName("Member_Export"));
        }

        public ActionResult ExportScoreHistory(SearchModel search)
        {
            List<ScoreHistory> scoreHistoryList = _helper.GetScoreList(search, o => o.UpdateTime, false);
            return File(ExcelHelper.SaveExcelContent(scoreHistoryList)
                        , ExcelHelper.Excel_ContentType
                        , ExcelHelper.GetExcelName("Member_ScoreHistory_Export"));
        }

        public async Task<ActionResult> Import(string excelPath)
        {
            List<Member> memberList = ExcelHelper.LoadExcel<Member>(excelPath.ToMapPath(), (row, cell, value) =>
            {
                if (cell == 3)
                {
                    //男
                    //Male = 1,
                    //女
                    //Famale = 0
                    switch (value.ToString().Trim())
                    {
                        case "男":
                        case "Male":
                        case "1":
                            return Sex.Male;

                        case "女":
                        case "Famale":
                        case "0":
                        default:
                            return Sex.Famale;
                    }
                }
                return value;
            }).ToList();

            await _helper.Import(memberList);
            return Success();
        }

        /// <summary>
        /// AutoScore
        /// </summary>
        /// <param name="id">Selected Month</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AutoScore(DateTime id)
        {
            DateTime endTime = id.AddMonths(1);
            if (DateTime.Now < endTime)
            {
                return Faild("该月还没有结束，不能进行优币奖励。");
            }

            if (await _helper.IsAutoScored(id))
            {
                return Faild("该月已经进行了优币奖励。");
            }

            bool result = await _helper.AutoScoreMonthly(id, endTime);
            if (result)
            {
                return Success();
            }

            return Faild("优币奖励失败，请稍后再试。");
        }
    }
}