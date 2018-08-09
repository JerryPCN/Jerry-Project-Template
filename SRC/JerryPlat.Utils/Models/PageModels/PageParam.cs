namespace JerryPlat.Utils.Models
{
    public class PageParam
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int GetPageSkip()
        {
            return (PageIndex - 1) * PageSize;
        }

        public void Check()
        {
            if (PageIndex <= 0)
            {
                PageIndex = 1;
            }

            if (PageSize <= 0)
            {
                PageSize = 20;
            }
        }
    }
}