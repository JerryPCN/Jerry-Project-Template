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

namespace JerryPlat.BLL.UserManage
{
    public class AdminUserHelper : BaseHelper
    {
        public async Task ClearData()
        {
            await _Db.Database.ExecuteSqlCommandAsync(@"
            truncate table AnswerRecord;
            truncate table Enroll;
            truncate table Exam;
            truncate table Member;
            truncate table MemberScoreHistory;
            truncate table PayRecord;
            truncate table QuestionFavorite;
            truncate table QuestionRecord;
            truncate table ScoreHistory;
            truncate table Subscribe;
            truncate table Withdraw;");
        }

        public async Task<AdminUser> GetAsync(LoginModel loginModel)
        {
            return await GetAsync(loginModel.UserName, loginModel.Password);
        }

        public Task<AdminUser> GetAsync(string strUserName, string strPassword)
        {
            return _Db.AdminUsers.Where(o => o.UserName == strUserName & o.Password == strPassword).FirstOrDefaultAsync();
        }

        public PageData<AdminUserDto> GetPageList(SearchModel searchModel, Expression<Func<AdminUser, int>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
        {
            IQueryable<AdminUser> adminUserQuerable = GetDbSet<AdminUser>().Where(o => o.GroupId > 0);
            PageData<AdminUser> adminUserList = PageHelper.GetPageData(adminUserQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);

            PageData<AdminUserDto> adminUserDtoList = new PageData<AdminUserDto>(adminUserList.PageParam, adminUserList.PageModel.TotalItem);

            List<int> groupIdList = adminUserList.Data.Select(o => o.GroupId).ToList();

            List<Group> groupList = GetDbSet<Group>().Where(o => groupIdList.Contains(o.Id)).ToList();

            adminUserDtoList.Data = adminUserList.Data.Select(o => new AdminUserDto
            {
                AdminUser = o,
                GroupName = groupList.Where(a => a.Id == o.GroupId).Select(a => a.Name).FirstOrDefault()
            }).ToList();

            return adminUserDtoList;
        }

        public async Task<bool> SaveAsync(AdminUser model)
        {
            if (model.Id == 0 || _Db.AdminUsers.Where(o => o.Id == model.Id && o.Password != model.Password).Any())
            {
                model.Password = EncryptHelper.Encrypt(model.Password);
            }

            return await SaveAsync<AdminUser>(model);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync<AdminUser>(id);
        }

        public async Task<bool> DeleteAsync(AdminUser entity)
        {
            return await DeleteAsync<AdminUser>(entity);
        }

        public async Task<bool> DeleteListAsync(List<int> idList)
        {
            return await DeleteListAsync<AdminUser>(idList);
        }

        public async Task<bool> ChangePassword(PasswordDto model)
        {
            if (model.Password != model.Confirm)
            {
                return false;
            }

            if (SessionHelper.Admin.IsNullSession())
            {
                return false;
            }

            AdminUser adminUser = SessionHelper.Admin.GetSession<AdminUser>();
            if (adminUser.Password != EncryptHelper.Encrypt(model.Original))
            {
                return false;
            }

            adminUser.Password = EncryptHelper.Encrypt(model.Password);

            Attach<AdminUser>(adminUser, EntityState.Modified);

            bool bIsSave = await SaveChangesAsync();
            if (bIsSave)
            {
                SessionHelper.Admin.SetSession(adminUser);
            }
            return bIsSave;
        }
    }
}