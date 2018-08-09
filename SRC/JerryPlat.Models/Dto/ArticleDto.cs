namespace JerryPlat.Models.Dto
{
    public class ArticleDto
    {
        public Article Article { get; set; }
        public string AdminUserName { get; set; }

        public ArticleDto()
        {
            Article = new Article();
        }
    }
}