using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JerryPlat.BLL.CommonManage
{
    public class QuestionTypeHelper : BaseHelper
    {
        public PageData<QuestionTypeDto> GetPageList(SearchModel searchModel, Expression<Func<QuestionType, int>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
        {
            IQueryable<QuestionType> questionTypeQuerable = GetDbSet<QuestionType>();

            PageData<QuestionType> questionTypeList = PageHelper.GetPageData(questionTypeQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);

            PageData<QuestionTypeDto> questionTypeDtoList = new PageData<QuestionTypeDto>(questionTypeList.PageParam, questionTypeList.PageModel.TotalItem);

            List<int> questionTypeIdList = questionTypeList.Data.Select(o => o.Id).ToList();

            List<QuestionChapter> chapterList = GetDbSet<QuestionChapter>().Where(o => questionTypeIdList.Contains(o.QuestionTypeId)).ToList();

            questionTypeDtoList.Data = questionTypeList.Data.Select(o => new QuestionTypeDto
            {
                QuestionType = o,
                QuestionChapters = chapterList.Where(a => a.QuestionTypeId == o.Id).OrderBy(a => a.OrderIndex).ToList()
            }).ToList();

            List<Question> questionList = GetDbSet<Question>().Where(o => questionTypeIdList.Contains(o.QuestionTypeId)).ToList();
            questionTypeDtoList.Data.ForEach(o =>
            {
                o.QuestionType.QuestionCount = questionList.Where(q => q.QuestionTypeId == o.QuestionType.Id).Count();
                o.QuestionChapters.ForEach(c =>
                {
                    c.QuestionCount = questionList.Where(q => q.QuestionTypeId == o.QuestionType.Id & q.QuestionChapterId == c.Id).Count();
                });
            });

            return questionTypeDtoList;
        }

        public async Task<bool> SaveAsync(QuestionTypeDto model)
        {
            if (model.QuestionType.Id == 0)
            {
                GetDbSet<QuestionType>().Add(model.QuestionType);
                await SaveChangesAsync();
            }
            else
            {
                Attach(model.QuestionType, EntityState.Modified);
            }

            List<int> questionChapterIdList = model.QuestionChapters.Where(o => o.Id > 0).Select(o => o.Id).ToList();

            var chapterDb = GetDbSet<QuestionChapter>();
            List<QuestionChapter> chapterList = chapterDb.Where(o => o.QuestionTypeId == model.QuestionType.Id && !questionChapterIdList.Contains(o.Id)).ToList();
            List<int> deleteChapterIdList = chapterList.Select(o => o.Id).ToList();

            List<Question> questionList = _Db.Questions.Where(o => o.QuestionTypeId == model.QuestionType.Id && deleteChapterIdList.Contains(o.QuestionChapterId)).ToList();
            _Db.Questions.RemoveRange(questionList);

            chapterDb.RemoveRange(chapterList);

            model.QuestionChapters.ForEach(o =>
            {
                o.QuestionTypeId = model.QuestionType.Id;
                if (o.Id > 0)
                {
                    Attach(o, EntityState.Modified);
                }
                else
                {
                    _Db.QuestionChapters.Add(o);
                }
            });
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            QuestionType entity = await GetByIdAsync<QuestionType>(id);
            return await DeleteAsync(entity);
        }

        public async Task<bool> DeleteAsync(QuestionType entity)
        {
            List<Question> quesionList = _Db.Questions.Where(o => o.QuestionTypeId == entity.Id).ToList();
            List<int> questionIdList = quesionList.Select(o => o.Id).ToList();
            _Db.Questions.RemoveRange(quesionList);

            List<Answer> answerList = _Db.Answers.Where(o => questionIdList.Contains(o.QuestionId)).ToList();
            _Db.Answers.RemoveRange(answerList);

            List<QuestionChapter> chapterList = _Db.QuestionChapters.Where(o => o.QuestionTypeId == entity.Id).ToList();
            _Db.QuestionChapters.RemoveRange(chapterList);

            GetDbSet<QuestionType>().Remove(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteListAsync(List<int> idList)
        {
            List<Question> quesionList = _Db.Questions.Where(o => idList.Contains(o.QuestionTypeId)).ToList();
            List<int> questionIdList = quesionList.Select(o => o.Id).ToList();
            _Db.Questions.RemoveRange(quesionList);

            List<Answer> answerList = _Db.Answers.Where(o => questionIdList.Contains(o.QuestionId)).ToList();
            _Db.Answers.RemoveRange(answerList);

            List<QuestionChapter> chapterList = _Db.QuestionChapters.Where(o => idList.Contains(o.QuestionTypeId)).ToList();
            _Db.QuestionChapters.RemoveRange(chapterList);

            List<QuestionType> entityList = GetByIdList<QuestionType>(idList);
            GetDbSet<QuestionType>().RemoveRange(entityList);
            return await SaveChangesAsync();
        }
    }
}