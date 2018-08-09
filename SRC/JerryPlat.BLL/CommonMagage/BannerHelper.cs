using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace JerryPlat.BLL.CommonMagage
{
    public class BannerHelper : BaseHelper<Banner>
    {
        public PageData<Banner> GetPageList(SearchModel searchModel, Expression<Func<Banner, int>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
        {
            IQueryable<Banner> bannerQueryable = base.GetQueryableList<Banner, SearchModel>(searchModel);

            if (searchModel.Id != 0)
            {
                bannerQueryable = bannerQueryable.Where(o => o.BannerType == (BannerType)searchModel.Id);
            }

            return PageHelper.GetPageData(bannerQueryable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);
        }
    }
}