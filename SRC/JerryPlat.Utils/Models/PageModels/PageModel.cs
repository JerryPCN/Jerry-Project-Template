using System;

namespace JerryPlat.Utils.Models
{
    public class PageModel
    {
        private PageParam _pageParam;

        public PageModel()
        {
            _pageParam = new PageParam();
        }

        public PageModel(PageParam pageParam, int totalItem)
        {
            this._pageParam = pageParam;
            this.TotalItem = totalItem;
        }

        public int TotalItem { get; set; }

        public int TotalPage
        {
            get
            {
                return (int)Math.Ceiling(1m * TotalItem / (_pageParam.PageSize == 0 ? 20 : _pageParam.PageSize));
            }
        }

        //public int PrevIndex
        //{
        //    get
        //    {
        //        return _pageParam.PageIndex > 1 ? _pageParam.PageIndex - 1 : 1;
        //    }
        //}
        //public int NextIndex
        //{
        //    get
        //    {
        //        return _pageParam.PageIndex < TotalPage ? _pageParam.PageIndex + 1 : TotalPage;
        //    }
        //}
    }
}