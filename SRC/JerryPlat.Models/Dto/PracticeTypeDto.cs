namespace JerryPlat.Models.Dto
{
    public class PracticeTypeDto
    {
        public int QuestionTypeId { get; set; }
        public int QuestionChapterId { get; set; }
        public int ExamId { get; set; }
        public int Different { get; set; }
        public PracticeType PracticeType { get; set; }
    }
}