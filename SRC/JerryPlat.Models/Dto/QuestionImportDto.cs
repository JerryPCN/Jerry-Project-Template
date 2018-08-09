using System.Collections.Generic;

namespace JerryPlat.Models.Dto
{
    public class ImportDto<T>
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }
    }

    public class ImportChapterDto
    {
        public int Chapter { get; set; }
        public int Count { get; set; }
        public int Id { get; set; }
        public int section1 { get; set; }
        public int section2 { get; set; }
        public int section3 { get; set; }
        public int section4 { get; set; }
        public string Title { get; set; }
    }

    public class ImportChapterQuestionDto
    {
        public ImportChapterDto Chapter { get; set; }
        public List<int> questionList { get; set; }
    }

    public class ImportQuestionDto
    {
        public int Answer { get; set; }
        public int ChapterId { get; set; }
        public int Difficulty { get; set; }
        public string Explain { get; set; }
        public int FalseCount { get; set; }
        public int id { get; set; }
        public string Label { get; set; }
        public string MediaContent { get; set; }
        public int MediaHeight { get; set; }
        public int MediaType { get; set; }
        public int MediaWidth { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string OptionE { get; set; }
        public string OptionF { get; set; }
        public string OptionG { get; set; }
        public string OptionH { get; set; }
        public int OptionType { get; set; }
        public string Question { get; set; }
        public int QuestionId { get; set; }
        public int TrueCount { get; set; }
        public decimal WrongRate { get; set; }
    }
}