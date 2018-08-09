using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JerryPlat.BLL.CommonMagage
{
    public class ArticleHelper : BaseHelper
    {
        public PageData<ArticleDto> GetPageList(SearchModel searchModel, Expression<Func<Article, DateTime>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
        {
            IQueryable<Article> articleQuerable = GetDbSet<Article>();
            if (searchModel.Id != 0)
            {
                articleQuerable = articleQuerable.Where(o => o.ArticleType == (ArticleType)searchModel.Id);
            }

            PageData<Article> articleList = PageHelper.GetPageData(articleQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);

            PageData<ArticleDto> articleDtoList = new PageData<ArticleDto>(articleList.PageParam, articleList.PageModel.TotalItem);

            List<int> adminUserIdList = articleList.Data.Select(o => o.AdminUserId).ToList();

            List<AdminUser> adminUserList = GetDbSet<AdminUser>().Where(o => adminUserIdList.Contains(o.Id)).ToList();

            articleDtoList.Data = articleList.Data.Select(o => new ArticleDto
            {
                Article = new Article
                {
                    Id = o.Id,
                    ArticleType = o.ArticleType,
                    AdminUserId = o.AdminUserId,
                    Title = o.Title,
                    UpdateTime = o.UpdateTime
                },
                AdminUserName = adminUserList.Where(a => a.Id == o.AdminUserId).Select(a => a.UserName).FirstOrDefault()
            }).ToList();

            return articleDtoList;
        }

        public async Task<Article> GetTopOneByArticleTypeAsync(ArticleType articleType)
        {
            return await _Db.Articles.Where(o => o.ArticleType == articleType)
                .OrderByDescending(o => o.UpdateTime).FirstOrDefaultAsync();
        }

        public async Task<Article> GetByIdAsync(int intId, bool bIsNotNull = true)
        {
            return await GetByIdAsync<Article>(intId, null, bIsNotNull);
        }

        public async Task<bool> SaveAsync(ArticleDto model)
        {
            AdminUser adminUser = SessionHelper.Admin.GetSession<AdminUser>();
            if (model.Article.AdminUserId == 0)
            {
                model.Article.AdminUserId = adminUser.Id;
            }
            //model.Article.Content = Sanitizer.GetSafeHtmlFragment(model.Article.Content);
            model.Article.UpdateTime = DateTime.Now;
            return await SaveAsync<Article>(model.Article);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Article entity = await GetByIdAsync<Article>(id);
            return await DeleteAsync(entity);
        }

        public async Task<bool> DeleteAsync(Article entity)
        {
            GetDbSet<Article>().Remove(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteListAsync(List<int> idList)
        {
            List<Article> entityList = GetByIdList<Article>(idList);
            GetDbSet<Article>().RemoveRange(entityList);
            return await SaveChangesAsync();
        }
    }
}