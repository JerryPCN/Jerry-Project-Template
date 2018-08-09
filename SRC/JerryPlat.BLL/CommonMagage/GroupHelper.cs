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
    public class GroupHelper : BaseHelper
    {
        public PageData<GroupDto> GetPageList(SearchModel searchModel, Expression<Func<Group, string>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
        {
            IQueryable<Group> groupQuerable = GetDbSet<Group>();
            PageData<Group> groupList = PageHelper.GetPageData(groupQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);

            PageData<GroupDto> groupDtoList = new PageData<GroupDto>(groupList.PageParam, groupList.PageModel.TotalItem);

            List<int> groupIdList = groupList.Data.Select(o => o.Id).ToList();

            List<Role> roleList = GetDbSet<Role>().Where(o => groupIdList.Contains(o.GroupId)).ToList();

            groupDtoList.Data = groupList.Data.Select(o => new GroupDto
            {
                Group = o,
                NavigationIdList = roleList.Where(a => a.GroupId == o.Id).Select(a => a.NavigationId).ToList()
            }).ToList();

            return groupDtoList;
        }

        public async Task<bool> SaveAsync(GroupDto model)
        {
            if (model.Group.Id == 0)
            {
                GetDbSet<Group>().Add(model.Group);
                await SaveChangesAsync();
            }
            else
            {
                Attach(model.Group, EntityState.Modified);
            }

            var roleDb = GetDbSet<Role>();
            List<Role> roleList = roleDb.Where(o => o.GroupId == model.Group.Id).ToList();
            roleDb.RemoveRange(roleList);

            model.NavigationIdList.ForEach(o =>
            {
                roleDb.Add(new Role
                {
                    GroupId = model.Group.Id,
                    NavigationId = o
                });
            });

            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Group entity = await GetByIdAsync<Group>(id);
            return await DeleteAsync(entity);
        }

        public async Task<bool> DeleteAsync(Group entity)
        {
            List<Role> RoleList = _Db.Roles.Where(o => o.GroupId == entity.Id).ToList();
            _Db.Roles.RemoveRange(RoleList);

            GetDbSet<Group>().Remove(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteListAsync(List<int> idList)
        {
            List<Role> RoleList = _Db.Roles.Where(o => idList.Contains(o.GroupId)).ToList();
            _Db.Roles.RemoveRange(RoleList);

            List<Group> entityList = GetByIdList<Group>(idList);
            GetDbSet<Group>().RemoveRange(entityList);
            return await SaveChangesAsync();
        }
    }
}