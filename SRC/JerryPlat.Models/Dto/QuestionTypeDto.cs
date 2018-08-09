using System.Collections.Generic;

namespace JerryPlat.Models.Dto
{
    public class QuestionTypeDto
    {
        public int ChapterCount { get { return QuestionChapters.Count; } }
        public QuestionType QuestionType { get; set; }
        public List<QuestionChapter> QuestionChapters { get; set; }

        public QuestionTypeDto()
        {
            QuestionType = new QuestionType();
            QuestionChapters = new List<QuestionChapter>();
        }
    }
}