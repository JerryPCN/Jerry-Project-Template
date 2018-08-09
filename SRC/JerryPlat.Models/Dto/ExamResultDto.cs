namespace JerryPlat.Models.Dto
{
    public class ExamResultDto : QuestionReportDto
    {
        public Exam Exam { get; set; }
        public string QuestionType { get; set; }
    }
}