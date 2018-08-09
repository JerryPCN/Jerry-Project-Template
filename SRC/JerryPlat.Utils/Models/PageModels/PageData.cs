using System.Collections.Generic;

namespace JerryPlat.Utils.Models
{
    public class PageData<T>
    {
        public PageData()
        {
        }

        public PageData(PageParam pageParam, int totalItem)
        {
            this.PageParam = pageParam;
            this.PageModel = new PageModel(pageParam, totalItem);
        }

        public List<T> Data { get; set; }
        public PageParam PageParam { get; set; }
        public PageModel PageModel { get; set; }
    }
}