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
    public class QuestionHelper : BaseHelper
    {
        public PageData<QuestionDto> GetPageList(SearchModel searchModel,
            Expression<Func<Question, int>> orderByKeySelector,
            PageParam pageParam,
            bool bIsAscOrder = true,
            List<int> selectedQuestionIdList = null,
            List<int> exceptQuestionIdList = null)
        {
            if (searchModel.Id == 0)
            {
                QuestionType questionType = GetDbSet<QuestionType>().FirstOrDefault();
                if (questionType != null)
                {
                    searchModel.Id = questionType.Id;
                }

                searchModel.Id1 = 0;
            }

            IQueryable<Question> questionQuerable = GetDbSet<Question>();
            if (searchModel.Id != 0)
            {
                questionQuerable = questionQuerable.Where(o => o.QuestionTypeId == searchModel.Id);
            }

            if (searchModel.Id1 != 0)
            {
                questionQuerable = questionQuerable.Where(o => o.QuestionChapterId == searchModel.Id1);
            }

            if (selectedQuestionIdList != null)
            {
                questionQuerable = questionQuerable.Where(o => selectedQuestionIdList.Contains(o.Id));
            }

            if (exceptQuestionIdList != null)
            {
                questionQuerable = questionQuerable.Where(o => !exceptQuestionIdList.Contains(o.Id));
            }

            PageData<Question> questionList = PageHelper.GetPageData(questionQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);

            PageData<QuestionDto> questionDtoList = new PageData<QuestionDto>(questionList.PageParam, questionList.PageModel.TotalItem);

            List<int> questionIdList = questionList.Data.Select(o => o.Id).ToList();

            List<Answer> answerList = GetDbSet<Answer>().Where(o => questionIdList.Contains(o.QuestionId)).ToList();

            questionDtoList.Data = questionList.Data.Select(o => new QuestionDto
            {
                Question = o,
                Answers = answerList.Where(a => a.QuestionId == o.Id).ToList()
            }).ToList();

            return questionDtoList;
        }

        public PageData<ExamResultDto> GetExamResultList(
                SearchModel searchModel,
                Expression<Func<Exam, DateTime>> orderByKeySelector,
                PageParam pageParam,
                bool bIsAscOrder = true)
        {
            IQueryable<Exam> examQuerable = GetDbSet<Exam>();

            if (searchModel.Id != 0)
            {
                examQuerable = examQuerable.Where(o => o.MemberId == searchModel.Id);
            }

            if (searchModel.Id1 != 0)
            {
                examQuerable = examQuerable.Where(o => o.QuestionTypeId == searchModel.Id1);
            }

            PageData<Exam> examList = PageHelper.GetPageData(examQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);

            PageData<ExamResultDto> examResultDtoList = new PageData<ExamResultDto>(examList.PageParam, examList.PageModel.TotalItem);

            List<int> questionTypeIdList = examList.Data.Select(o => o.QuestionTypeId).ToList();
            List<QuestionType> questionTypeList = GetDbSet<QuestionType>().Where(o => questionTypeIdList.Contains(o.Id)).ToList();

            List<int> examIdList = examList.Data.Select(o => o.Id).ToList();
            List<AnswerRecord> answerRecordList = GetDbSet<AnswerRecord>().Where(o => examIdList.Contains(o.ExamId)).ToList();

            examResultDtoList.Data = examList.Data.Select(o => new ExamResultDto
            {
                Exam = o,
                QuestionType = questionTypeList.Where(q => q.Id == o.QuestionTypeId).Select(q => q.Name).FirstOrDefault(),
                ErrorCount = answerRecordList.Where(a => a.ExamId == o.Id).Count(a => a.AnswerStatus == AnswerStatus.Error),
                CurrectCount = answerRecordList.Where(a => a.ExamId == o.Id).Count(a => a.AnswerStatus == AnswerStatus.Correct),
                IgnoreCount = answerRecordList.Where(a => a.ExamId == o.Id).Count(a => a.AnswerStatus == AnswerStatus.Ignore)
            }).ToList();

            return examResultDtoList;
        }

        public async Task<bool> SaveAsync(QuestionDto model)
        {
            if (model.Question.Id == 0)
            {
                GetDbSet<Question>().Add(model.Question);
                await SaveChangesAsync();
            }
            else
            {
                Attach(model.Question, EntityState.Modified);
            }

            var answerDb = GetDbSet<Answer>();
            List<Answer> answerList = answerDb.Where(o => o.QuestionId == model.Question.Id).ToList();
            answerDb.RemoveRange(answerList);

            model.Answers.ForEach(o => o.QuestionId = model.Question.Id);
            answerDb.AddRange(model.Answers);

            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Question entity = await GetByIdAsync<Question>(id);
            return await DeleteAsync(entity);
        }

        public async Task<bool> DeleteAsync(Question entity)
        {
            List<Answer> answerList = _Db.Answers.Where(o => o.QuestionId == entity.Id).ToList();
            _Db.Answers.RemoveRange(answerList);

            GetDbSet<Question>().Remove(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteListAsync(List<int> idList)
        {
            List<Answer> answerList = _Db.Answers.Where(o => idList.Contains(o.QuestionId)).ToList();
            _Db.Answers.RemoveRange(answerList);

            List<Question> entityList = GetByIdList<Question>(idList);
            GetDbSet<Question>().RemoveRange(entityList);
            return await SaveChangesAsync();
        }

        public async Task<QuestionRecord> GetQuestoinRecordAsync(PracticeTypeDto model)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            return await GetQuestoinRecordAsync(member, model);
        }

        public async Task<QuestionRecord> GetQuestoinRecordAsync(Member member, PracticeTypeDto model)
        {
            return await _Db.QuestionRecords.Where(o => o.MemberId == member.Id
                                        & o.QuestionTypeId == model.QuestionTypeId
                                        & o.QuestionChapterId == model.QuestionChapterId
                                        & o.ExamId == model.ExamId
                                        & o.PracticeType == model.PracticeType).FirstOrDefaultAsync();
        }

        public async Task<int> SetExam(int intQuestoinTypeId)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            DateTime datetimeNow = DateTime.Now;
            Exam exam = new Exam
            {
                MemberId = member.Id,
                QuestionTypeId = intQuestoinTypeId,
                StartTime = datetimeNow,
                EndTime = datetimeNow.AddMinutes(45),
                IsEnd = false,
                TotalMark = 0
            };
            await SaveAsync(exam);
            await SetQuestionRecordAsync(member, new PracticeTypeDto
            {
                QuestionTypeId = intQuestoinTypeId,
                QuestionChapterId = 0,
                PracticeType = PracticeType.Exam,
                ExamId = exam.Id,
                Different = 0
            });
            return exam.Id;
        }

        public async Task<QuestionRecord> SetQuestionRecordAsync(PracticeTypeDto model)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            return await SetQuestionRecordAsync(member, model);
        }

        public async Task<QuestionRecord> SetQuestionRecordAsync(Member member, PracticeTypeDto model)
        {
            QuestionRecord record = (await GetQuestoinRecordAsync(member, model))
                                    ?? new QuestionRecord
                                    {
                                        MemberId = member.Id,
                                        QuestionTypeId = model.QuestionTypeId,
                                        QuestionChapterId = model.QuestionChapterId,
                                        ExamId = model.ExamId,
                                        PracticeType = model.PracticeType,
                                        CurrentIndex = 0
                                    };
            record.UpdateTime = DateTime.Now;
            await SaveAsync(record);
            return record;
        }

        private async Task<List<int>> GetSelectedQuestionIdListAsync(PracticeTypeDto model)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            return await GetSelectedQuestionIdListAsync(member, model);
        }

        private async Task<List<int>> GetSelectedQuestionIdListAsync(Member member, PracticeTypeDto model)
        {
            switch (model.PracticeType)
            {
                case PracticeType.Favorite:
                    return await _Db.QuestionFavorites.Where(o => o.MemberId == member.Id
                                    & o.QuestionTypeId == model.QuestionTypeId
                                    & o.IsFavorite
                                ).Select(o => o.QuestionId).Distinct().ToListAsync();

                case PracticeType.Error:
                case PracticeType.Ignore:
                    AnswerStatus answerStatus = model.PracticeType == PracticeType.Error ? AnswerStatus.Error : AnswerStatus.Ignore;
                    IQueryable<AnswerRecord> answerRecordsQueryable = _Db.AnswerRecords.Where(o => o.MemberId == member.Id
                                    & o.AnswerStatus == answerStatus);
                    if (model.ExamId != 0)
                    {
                        answerRecordsQueryable = answerRecordsQueryable.Where(o => o.ExamId == model.ExamId);
                    }
                    return await (from a in answerRecordsQueryable
                                  join b in _Db.Questions.Where(o => o.QuestionTypeId == model.QuestionTypeId) on a.QuestionId equals b.Id
                                  select a.QuestionId
                                  ).Distinct().ToListAsync();

                case PracticeType.Exam:
                case PracticeType.Order:
                case PracticeType.Random:
                default:
                    return null;
            }
        }

        private async Task<List<int>> GetExceptQuestionIdListAsync(PracticeTypeDto model)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            return await GetExceptQuestionIdListAsync(member, model);
        }

        private async Task<List<int>> GetExceptQuestionIdListAsync(Member member, PracticeTypeDto model)
        {
            switch (model.PracticeType)
            {
                case PracticeType.Exam:
                    IQueryable<AnswerRecord> answerRecordsQueryable = _Db.AnswerRecords.Where(o => o.MemberId == member.Id
                                                & o.ExamId == model.ExamId);
                    return await (from a in answerRecordsQueryable
                                  join b in _Db.Questions.Where(o => o.QuestionTypeId == model.QuestionTypeId) on a.QuestionId equals b.Id
                                  select a.QuestionId
                                  ).Distinct().ToListAsync();

                case PracticeType.Favorite:
                case PracticeType.Error:
                case PracticeType.Ignore:
                case PracticeType.Order:
                case PracticeType.Random:
                default:
                    return null;
            }
        }

        private async Task<int> GetQuestionCount(PracticeTypeDto model, List<int> selectedQuestionIdList = null, List<int> exceptQuestionIdList = null)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            return await GetQuestionCount(member, model, selectedQuestionIdList, exceptQuestionIdList);
        }

        private async Task<int> GetQuestionCount(Member member, PracticeTypeDto model, List<int> selectedQuestionIdList = null, List<int> exceptQuestionIdList = null)
        {
            switch (model.PracticeType)
            {
                case PracticeType.Favorite:
                case PracticeType.Error:
                case PracticeType.Ignore:
                    return (selectedQuestionIdList ?? (await GetSelectedQuestionIdListAsync(member, model))).Count;

                case PracticeType.Exam:
                case PracticeType.Order:
                case PracticeType.Random:
                default:
                    int intTotalCount = (await _Db.Questions.Where(o => o.QuestionTypeId == model.QuestionTypeId).CountAsync());
                    if (model.PracticeType == PracticeType.Exam && exceptQuestionIdList != null)
                    {
                        intTotalCount = intTotalCount - exceptQuestionIdList.Count;
                    }
                    return intTotalCount;
            }
        }

        private async Task<int> GetPageIndexAsync(PracticeTypeDto model, List<int> selectedQuestionIdList = null, List<int> exceptQuestionIdList = null)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            return await GetPageIndexAsync(member, model, selectedQuestionIdList, exceptQuestionIdList);
        }

        private async Task<int> GetPageIndexAsync(Member member, PracticeTypeDto model, List<int> selectedQuestionIdList = null, List<int> exceptQuestionIdList = null)
        {
            QuestionRecord record = await SetQuestionRecordAsync(member, model);
            switch (model.PracticeType)
            {
                case PracticeType.Order:
                case PracticeType.Favorite:
                case PracticeType.Error:
                case PracticeType.Ignore:
                    return record.CurrentIndex + model.Different;

                case PracticeType.Random:
                case PracticeType.Exam:
                    int intQuestionCount = await GetQuestionCount(member, model, selectedQuestionIdList, exceptQuestionIdList);
                    return new Random(Guid.NewGuid().GetHashCode()).Next(1, intQuestionCount + 1);

                default:
                    return 1;
            }
        }

        public async Task<PageData<QuestionDto>> GetQuestionAsync(PracticeTypeDto model)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();

            List<int> selectedQuestionIdList = await GetSelectedQuestionIdListAsync(member, model);
            List<int> exceptQuestionIdList = await GetExceptQuestionIdListAsync(member, model);

            return GetPageList(new SearchModel
            {
                Id = model.QuestionTypeId,
                Id1 = model.QuestionChapterId
            }, o => o.OrderIndex, new PageParam
            {
                PageIndex = await GetPageIndexAsync(member, model, selectedQuestionIdList, exceptQuestionIdList),
                PageSize = 1
            },
            true,
            selectedQuestionIdList,
            exceptQuestionIdList);
        }

        private async Task<int> GetQuestionRecordIdAsync(PracticeTypeDto model)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            return await GetQuestionRecordIdAsync(member, model);
        }

        private async Task<QuestionRecord> GetQuestionRecordAsync(Member member, PracticeTypeDto model)
        {
            QuestionRecord record = await GetQuestoinRecordAsync(member, model);
            if (record == null)
            {
                throw new Exception("Not Exist QuestionRecord where QuestionTypeId=" + model.QuestionTypeId + " & QuestionChapterId = " + model.QuestionChapterId);
            }
            return record;
        }

        private async Task<int> GetQuestionRecordIdAsync(Member member, PracticeTypeDto model)
        {
            QuestionRecord record = await GetQuestoinRecordAsync(member, model);
            return record.Id;
        }

        public async Task<List<AnswerRecord>> GetAnswerRecordListAsync(PracticeTypeDto model)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            int intQuestionRecordId = await GetQuestionRecordIdAsync(member, model);

            return await _Db.AnswerRecords.Where(o => o.MemberId == member.Id
                     & o.QuestionRecordId == intQuestionRecordId
                     & o.ExamId == model.ExamId).ToListAsync();
        }

        public async Task<QuestionReportDto> GetQuestionReportAsync(PracticeTypeDto model)
        {
            List<AnswerRecord> anwserRecordList = await GetAnswerRecordListAsync(model);

            List<int> questionIdList = anwserRecordList.Where(o => o.AnswerStatus == AnswerStatus.Correct)
                                    .Select(o => o.QuestionId).ToList();

            QuestionReportDto dto = new QuestionReportDto
            {
                ErrorCount = anwserRecordList.Count(o => o.AnswerStatus == AnswerStatus.Error),
                CurrectCount = anwserRecordList.Count(o => o.AnswerStatus == AnswerStatus.Correct),
                IgnoreCount = anwserRecordList.Count(o => o.AnswerStatus == AnswerStatus.Ignore)
            };

            if (questionIdList.Any())
            {
                dto.Marks = _Db.Questions.Where(o => questionIdList.Contains(o.Id)).Select(o => o.Mark).DefaultIfEmpty().Sum();
            }

            return dto;
        }

        public async Task<bool> AnswerAsync(PracticeTypeDto practiceModel, PageData<QuestionDto> pageQuestionModel)
        {
            if (!pageQuestionModel.Data.Any())
            {
                return false;
            }

            Member member = SessionHelper.Mob.GetSession<Member>();

            QuestionRecord questionRecord = await GetQuestionRecordAsync(member, practiceModel);

            bool bIsCurrect = false;
            foreach (QuestionDto dto in pageQuestionModel.Data)
            {
                bIsCurrect = (await Answer(member, questionRecord.Id, practiceModel, dto));

                if (!(practiceModel.PracticeType == PracticeType.Ignore
                    || (bIsCurrect && practiceModel.PracticeType == PracticeType.Error)
                    ))
                {
                    if (practiceModel.PracticeType == PracticeType.Exam)
                    {
                        questionRecord.CurrentIndex = questionRecord.CurrentIndex + 1;
                    }
                    else
                    {
                        questionRecord.CurrentIndex = pageQuestionModel.PageParam.PageIndex;
                    }
                }
            }

            return await SaveChangesAsync();
        }

        private async Task<List<AnswerRecord>> GetAnswerRecordListAsync(Member member, int intQuestionRecordId, PracticeTypeDto practiceModel, QuestionDto questionModel)
        {
            List<AnswerRecord> answerRecordList = null;

            switch (practiceModel.PracticeType)
            {
                case PracticeType.Error:
                case PracticeType.Ignore:
                    AnswerStatus answerStatus = practiceModel.PracticeType == PracticeType.Error ? AnswerStatus.Error : AnswerStatus.Ignore;
                    answerRecordList = await _Db.AnswerRecords.Where(o => o.MemberId == member.Id
                                     & o.QuestionId == questionModel.Question.Id
                                     & o.AnswerStatus == answerStatus).ToListAsync();
                    break;

                default:
                    answerRecordList = await _Db.AnswerRecords.Where(o => o.MemberId == member.Id
                                     & o.QuestionRecordId == intQuestionRecordId
                                     & o.ExamId == practiceModel.ExamId
                                     & o.QuestionId == questionModel.Question.Id).ToListAsync();
                    if (!answerRecordList.Any())
                    {
                        answerRecordList.Add(new AnswerRecord
                        {
                            MemberId = member.Id,
                            QuestionRecordId = intQuestionRecordId,
                            ExamId = practiceModel.ExamId,
                            QuestionId = questionModel.Question.Id,
                            UpdateTime = DateTime.Now
                        });
                    }
                    break;
            }
            return answerRecordList;
        }

        public async Task<bool> Answer(Member member, int intQuestionRecordId, PracticeTypeDto practiceModel, QuestionDto questionModel)
        {
            List<AnswerRecord> answerRecordList = await GetAnswerRecordListAsync(member, intQuestionRecordId, practiceModel, questionModel);

            string strAnswer = GetAnswer(questionModel);
            bool bIsCurrect = false;
            foreach (AnswerRecord answerRecord in answerRecordList)
            {
                if (!string.IsNullOrEmpty(strAnswer) || string.IsNullOrEmpty(answerRecord.Answer))
                {
                    answerRecord.Answer = GetAnswer(questionModel);
                    answerRecord.AnswerStatus = GetAnswerStatus(questionModel);

                    if (answerRecord.AnswerStatus == AnswerStatus.Correct)
                    {
                        bIsCurrect = true;
                    }
                }

                if (answerRecord.Id == 0)
                {
                    _Db.AnswerRecords.Add(answerRecord);
                }
            }

            await SaveChangesAsync();
            return bIsCurrect;
        }

        public async Task<bool> SetFavoriteAsync(PracticeTypeDto practiceModel, List<QuestionDto> questionModel)
        {
            if (!questionModel.Any())
            {
                return false;
            }

            Member member = SessionHelper.Mob.GetSession<Member>();

            foreach (QuestionDto dto in questionModel)
            {
                if (!(await SetFavoriteAsync(member, practiceModel, dto)))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> CheckFavoriteAsync(PracticeTypeDto practiceModel, List<QuestionDto> questionModel)
        {
            if (!questionModel.Any())
            {
                return false;
            }

            Member member = SessionHelper.Mob.GetSession<Member>();

            List<int> questionIdList = questionModel.Select(o => o.Question.Id).ToList();

            return await _Db.QuestionFavorites.Where(o => o.MemberId == member.Id
                            & o.QuestionTypeId == practiceModel.QuestionTypeId
                            & questionIdList.Contains(o.QuestionId)
                            & o.IsFavorite).AnyAsync();
        }

        public async Task<bool> SetFavoriteAsync(Member member, PracticeTypeDto practiceModel, QuestionDto questionModel)
        {
            QuestionFavorite questionFavorite = (await _Db.QuestionFavorites.Where(o => o.MemberId == member.Id
                                         && o.QuestionId == questionModel.Question.Id).FirstOrDefaultAsync())
                                        ?? new QuestionFavorite
                                        {
                                            MemberId = member.Id,
                                            QuestionTypeId = practiceModel.QuestionTypeId,
                                            QuestionChapterId = practiceModel.QuestionChapterId,
                                            QuestionId = questionModel.Question.Id,
                                            IsFavorite = false
                                        };
            questionFavorite.IsFavorite = !questionFavorite.IsFavorite;
            return await SaveAsync(questionFavorite);
        }

        public async Task<bool> Reset(PracticeTypeDto practiceModel)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            QuestionRecord record = await GetQuestoinRecordAsync(member, practiceModel);
            if (record == null)
            {
                throw new Exception("Not Exist QuestionRecord where QuestionTypeId=" + practiceModel.QuestionTypeId + " & QuestionChapterId = " + practiceModel.QuestionChapterId);
            }

            record.CurrentIndex = 0;
            return await SaveChangesAsync();
        }

        private bool IsSingleSelected(QuestionDto questionModel)
        {
            return IsSingleSelected(questionModel.Question.AnswerType);
        }

        private bool IsSingleSelected(AnswerType answerType)
        {
            switch (answerType)
            {
                case AnswerType.Judgement:
                case AnswerType.Single:
                    return true;

                default:
                    return false;
            }
        }

        private string GetAnswer(QuestionDto questionModel)
        {
            return IsSingleSelected(questionModel.Question.AnswerType) ? questionModel.QuestionAnswer : string.Join(",", questionModel.QuestionAnswerList.OrderBy(o => o).ToList());
        }

        private AnswerStatus GetAnswerStatus(QuestionDto questionModel)
        {
            bool bIsSingle = IsSingleSelected(questionModel.Question.AnswerType);

            if ((bIsSingle && string.IsNullOrEmpty(questionModel.QuestionAnswer))
                || (!bIsSingle && !questionModel.QuestionAnswerList.Any()))
            {
                return AnswerStatus.Ignore;
            }

            List<string> currectList = new List<string>();
            foreach (var item in questionModel.Answers)
            {
                if (item.IsAnswer)
                {
                    currectList.Add(item.Id.ToString());
                }
            }

            currectList = currectList.OrderBy(o => o).ToList();

            if (GetAnswer(questionModel) == string.Join(",", currectList))
            {
                return AnswerStatus.Correct;
            }

            return AnswerStatus.Error;
        }

        public PageData<QuestionChapter> GetQuestionChapter(SearchModel searchModel, PageParam pageParam)
        {
            IQueryable<QuestionChapter> questionChapterQuerable = _Db.QuestionChapters.Where(o => o.QuestionTypeId == searchModel.Id);
            return PageHelper.GetPageData<QuestionChapter, int>(questionChapterQuerable, o => o.OrderIndex, pageParam, true, searchModel.Sort);
        }

        public async Task<int> EndExamAsync(int intExamId)
        {
            Member member = SessionHelper.Mob.GetSession<Member>();
            Exam exam = await _Db.Exams.Where(o => o.MemberId == member.Id & o.Id == intExamId).FirstOrDefaultAsync();
            if (exam == null)
            {
                throw new Exception("Not exist Exam with Id = " + intExamId);
            }

            exam.EndTime = DateTime.Now;
            exam.IsEnd = true;

            QuestionRecord questionRecord = await GetQuestionRecordAsync(member, new PracticeTypeDto
            {
                QuestionTypeId = exam.QuestionTypeId,
                QuestionChapterId = 0,
                ExamId = exam.Id,
                PracticeType = PracticeType.Exam
            });

            exam.TotalMark = (from a in _Db.AnswerRecords.Where(o => o.MemberId == member.Id
                                            & o.QuestionRecordId == questionRecord.Id
                                            & o.ExamId == exam.Id
                                            & o.AnswerStatus == AnswerStatus.Correct)
                              join b in _Db.Questions.Where(o => o.QuestionTypeId == exam.QuestionTypeId) on a.QuestionId equals b.Id
                              select b.Mark).DefaultIfEmpty().Sum();
            await SaveChangesAsync();
            return exam.TotalMark;
        }

        #region Import

        public async Task<bool> Import()
        {
            await _Db.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE Question;TRUNCATE TABLE QuestionChapter;TRUNCATE TABLE Answer;");

            List<QuestionType> questionTypeList = _Db.QuestionTypes.OrderBy(o => o.OrderIndex).ToList();

            foreach (QuestionType questionType in questionTypeList)
            {
                if (!(await Import(questionType)))
                {
                    return false;
                }
            }
            return true;
        }

        private async Task<bool> Import(QuestionType questionType)
        {
            if (string.IsNullOrEmpty(questionType.Source))
            {
                return true;
            }

            try
            {
                ImportDto<List<ImportChapterDto>> importDto = HttpHelper.Get<ImportDto<List<ImportChapterDto>>>(questionType.Source);

                if (!importDto.Success)
                {
                    return false;
                }

                foreach (ImportChapterDto importChapterDto in importDto.Data)
                {
                    if (!(await Import(questionType, importChapterDto)))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        private async Task<bool> Import(QuestionType questionType, ImportChapterDto importChapterDto)
        {
            try
            {
                QuestionChapter questionChapter = new QuestionChapter
                {
                    QuestionTypeId = questionType.Id,
                    Name = importChapterDto.Title,
                    OrderIndex = importChapterDto.Chapter
                };

                _Db.QuestionChapters.Add(questionChapter);
                await _Db.SaveChangesAsync();

                ImportDto<ImportChapterQuestionDto> importDto = HttpHelper.Get<ImportDto<ImportChapterQuestionDto>>("https://api2.jiakaobaodian.com/api/open/exercise/chapter.htm?_appName=jiakaobaodian&_platform=wap&_r=110028075929574772092&carType=car&chapterId=" + importChapterDto.Id + "&cityCode=430100&course=kemu1");
                if (!importDto.Success)
                {
                    return false;
                }

                int intOrderIndex = 0;

                List<int> questionList = new List<int>();

                foreach (int question in importDto.Data.questionList)
                {
                    if (questionList.Count >= 50)
                    {
                        await Import(questionType, questionChapter, questionList, intOrderIndex);
                        intOrderIndex += questionList.Count;
                        questionList.Clear();
                    }
                    questionList.Add(question);
                }

                await Import(questionType, questionChapter, questionList, intOrderIndex);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        private async Task<bool> Import(QuestionType questionType, QuestionChapter questionChapter, List<int> questionList, int intOrderIndex)
        {
            if (!questionList.Any())
            {
                return true;
            }

            try
            {
                ImportDto<List<ImportQuestionDto>> importDto = HttpHelper.Get<ImportDto<List<ImportQuestionDto>>>("http://api2.jiakaobaodian.com/api/open/question/question-list.htm?_r=114972216825686778106&questionIds=" + string.Join("%2C", questionList) + "&_=0.8455804460813621");
                if (!importDto.Success)
                {
                    return false;
                }

                foreach (ImportQuestionDto importQuestionDto in importDto.Data)
                {
                    if (!(await Import(questionType, questionChapter, importQuestionDto, intOrderIndex++)))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        private const string CharAnswer = "0000ABCD";

        private string GetCharAnswer(int intAnswer)
        {
            string strResult = "";
            if (intAnswer < 16)
            {
                return strResult;
            }

            string strAnswer = Convert.ToString(intAnswer, 2);
            for (int index = 0; index < strAnswer.Length; index++)
            {
                if (strAnswer[index] == '1')
                {
                    strResult += CharAnswer[strAnswer.Length - index - 1];
                }
            }
            return strResult;
        }

        private async Task<bool> Import(QuestionType questionType, QuestionChapter questionChapter, ImportQuestionDto importQuestionDto, int intOrderIndex)
        {
            string strAnswer = GetCharAnswer(importQuestionDto.Answer);

            QuestionDto model = new QuestionDto
            {
                Question = new Question
                {
                    QuestionTypeId = questionType.Id,
                    QuestionChapterId = questionChapter.Id,
                    Description = importQuestionDto.Question,
                    PicPath = importQuestionDto.MediaContent,
                    Explanation = importQuestionDto.Explain,
                    OrderIndex = intOrderIndex,
                    Mark = 1,
                    AnswerType = (AnswerType)(importQuestionDto.OptionType + 1)
                },
                Answers = new List<Answer>
                        {
                            new Answer
                            {
                                Description = importQuestionDto.OptionA,
                                OrderIndex = 1,
                                IsAnswer = strAnswer.Contains("A")
                            },
                            new Answer
                            {
                                Description = importQuestionDto.OptionB,
                                OrderIndex = 2,
                                IsAnswer = strAnswer.Contains("B")
                            }
                        }
            };

            if (!string.IsNullOrEmpty(importQuestionDto.OptionC))
            {
                model.Answers.Add(new Answer
                {
                    Description = importQuestionDto.OptionC,
                    OrderIndex = 3,
                    IsAnswer = strAnswer.Contains("C")
                });
            }

            if (!string.IsNullOrEmpty(importQuestionDto.OptionD))
            {
                model.Answers.Add(new Answer
                {
                    Description = importQuestionDto.OptionD,
                    OrderIndex = 4,
                    IsAnswer = strAnswer.Contains("D")
                });
            }

            if (!string.IsNullOrEmpty(importQuestionDto.OptionE))
            {
                model.Answers.Add(new Answer
                {
                    Description = importQuestionDto.OptionE,
                    OrderIndex = 5,
                    IsAnswer = strAnswer.Contains("E")
                });
            }

            if (!string.IsNullOrEmpty(importQuestionDto.OptionF))
            {
                model.Answers.Add(new Answer
                {
                    Description = importQuestionDto.OptionF,
                    OrderIndex = 6,
                    IsAnswer = strAnswer.Contains("F")
                });
            }

            if (!string.IsNullOrEmpty(importQuestionDto.OptionG))
            {
                model.Answers.Add(new Answer
                {
                    Description = importQuestionDto.OptionG,
                    OrderIndex = 7,
                    IsAnswer = strAnswer.Contains("G")
                });
            }

            if (!string.IsNullOrEmpty(importQuestionDto.OptionH))
            {
                model.Answers.Add(new Answer
                {
                    Description = importQuestionDto.OptionH,
                    OrderIndex = 8,
                    IsAnswer = strAnswer.Contains("H")
                });
            }

            return await SaveAsync(model);
        }

        #endregion Import
    }
}