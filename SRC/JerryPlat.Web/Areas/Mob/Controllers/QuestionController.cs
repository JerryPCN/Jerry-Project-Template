using JerryPlat.BLL.CommonManage;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Mob.Controllers
{
    public class QuestionController : AuthurizeBaseController<QuestionHelper>
    {
        /// <summary>
        /// 顺序练习
        /// </summary>
        /// <param name="id">QuestionTypeId</param>
        /// <returns></returns>
        public ActionResult Order(int id, int questionChapterId = 0, int practiceType = 1, int examId = 0)
        {
            ViewBag.QuestionChapterId = questionChapterId;
            ViewBag.PracticeType = practiceType;
            ViewBag.ExamId = examId;
            return View(id);
        }

        /// <summary>
        /// 随机练习
        /// </summary>
        /// <param name="id">QuestionTypeId</param>
        /// <returns></returns>
        public ActionResult Random(int id)
        {
            return View(id);
        }

        /// <summary>
        /// 随机练习
        /// </summary>
        /// <param name="id">QuestionTypeId</param>
        /// <returns></returns>
        public ActionResult Chapter(int id)
        {
            return View(id);
        }

        /// <summary>
        /// 真题考试
        /// </summary>
        /// <param name="id">QuestionTypeId</param>
        /// <returns></returns>
        public async Task<ActionResult> Exam(int id)
        {
            ViewBag.ExamId = await _helper.SetExam(id);
            return View(id);
        }

        public ActionResult Result()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetQuestionChapterList(SearchModel searchModel, PageParam pageParam)
        {
            return Success(_helper.GetQuestionChapter(searchModel, pageParam));
        }

        [HttpPost]
        public async Task<ActionResult> GetQuestion(PracticeTypeDto model)
        {
            return Success(await _helper.GetQuestionAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> GetQuestionReport(PracticeTypeDto model)
        {
            return Success(await _helper.GetQuestionReportAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> Answer(PracticeTypeDto practiceModel, PageData<QuestionDto> pageQuestionModel)
        {
            return Success(await _helper.AnswerAsync(practiceModel, pageQuestionModel));
        }

        [HttpPost]
        public async Task<ActionResult> CheckFavorite(PracticeTypeDto practiceModel, List<QuestionDto> questionModel)
        {
            return Success(await _helper.CheckFavoriteAsync(practiceModel, questionModel));
        }

        [HttpPost]
        public async Task<ActionResult> SetFavorite(PracticeTypeDto practiceModel, List<QuestionDto> questionModel)
        {
            return Success(await _helper.SetFavoriteAsync(practiceModel, questionModel));
        }

        [HttpPost]
        public async Task<ActionResult> Reset(PracticeTypeDto practiceModel)
        {
            return Success(await _helper.Reset(practiceModel));
        }

        [HttpPost]
        public async Task<ActionResult> EndExam(int id)
        {
            return Success(await _helper.EndExamAsync(id));
        }

        [HttpPost]
        public ActionResult GetResultList(PageParam pageParam)
        {
            PageData<ExamResultDto> dto = _helper.GetExamResultList(new SearchModel { Id = _Member.Id }, o => o.StartTime, pageParam, false);
            return Success(dto);
        }
    }
}