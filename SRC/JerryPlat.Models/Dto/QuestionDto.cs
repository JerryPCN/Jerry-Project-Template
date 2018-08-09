using System.Collections.Generic;

namespace JerryPlat.Models.Dto
{
    public class QuestionDto
    {
        public int AnswerCount { get { return Answers.Count; } }
        public Question Question { get; set; }
        public List<Answer> Answers { get; set; }
        public string QuestionAnswer { get; set; }
        public List<string> QuestionAnswerList { get; set; }

        public QuestionDto()
        {
            Question = new Question();
            Answers = new List<Answer>();
            QuestionAnswer = string.Empty;
            QuestionAnswerList = new List<string>();
        }
    }
}