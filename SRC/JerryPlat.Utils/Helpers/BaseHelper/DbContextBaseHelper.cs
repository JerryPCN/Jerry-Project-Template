using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JerryPlat.Utils.Helpers
{
    public class DbContextBaseHelper<TDbContext> : IDisposable where TDbContext : DbContext, new()
    {
        #region ORM

        private TDbContext db;
        protected TDbContext _Db => db ?? (db = SingleInstanceHelper.GetInstance<TDbContext>());

        public virtual DbSet<TEntity> GetDbSet<TEntity>()
            where TEntity : class, new()
        {
            return _Db.Set<TEntity>();
        }

        protected virtual void CheckEntityIfDiffThenMustOverride()
        {
        }

        public virtual void Attach<TEntity>(TEntity entity)
            where TEntity : class, new()
        {
            GetDbSet<TEntity>().Attach(entity);
        }

        public virtual void Attach<TEntity>(TEntity entity, EntityState entityState)
              where TEntity : class, new()
        {
            Attach(GetDbSet<TEntity>(), entity, entityState);
        }

        public virtual void Attach<TEntity>(DbSet<TEntity> entities, TEntity entity, EntityState entityState)
               where TEntity : class, new()
        {
            entities.Attach(entity);
            _Db.Entry(entity).State = entityState;
        }

        public virtual IQueryable<TEntity> GetQueryableList<TEntity>(IQueryable<TEntity> queryEntity = null)
           where TEntity : class, new()
        {
            CheckEntityIfDiffThenMustOverride();

            return queryEntity ?? (GetDbSet<TEntity>().AsNoTracking() as IQueryable<TEntity>);
        }

        public virtual IQueryable<TEntity> GetQueryableList<TEntity, TSearchModel>(TSearchModel seachModel, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
        {
            CheckEntityIfDiffThenMustOverride();

            return queryEntity ?? (GetDbSet<TEntity>().AsNoTracking() as IQueryable<TEntity>);
        }

        public virtual List<TEntity> GetList<TEntity>(IQueryable<TEntity> queryEntity = null)
         where TEntity : class, new()
        {
            queryEntity = GetQueryableList<TEntity>(queryEntity);
            return queryEntity.ToList();
        }

        public virtual List<TEntity> GetList<TEntity, TSearchModel>(TSearchModel seachModel, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
        {
            queryEntity = GetQueryableList<TEntity, TSearchModel>(seachModel, queryEntity);
            return queryEntity.ToList();
        }

        public virtual PageData<TEntity> GetPageList<TEntity, TSearchModel>(TSearchModel searchModel, PageParam pageParam, bool bIsAscOrder = true, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
            where TSearchModel : ISearchModel
        {
            Expression<Func<TEntity, int>> keySelector = PageHelper.GetDefaultKeyExpression<TEntity, int>();
            IQueryable<TEntity> queryList = GetQueryableList<TEntity, TSearchModel>(searchModel, queryEntity);
            return PageHelper.GetPageData(queryList, keySelector, pageParam, bIsAscOrder, searchModel?.Sort);
        }

        public virtual PageData<TEntity> GetPageList<TEntity, TKey, TSearchModel>(TSearchModel searchModel, Expression<Func<TEntity, TKey>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true, IQueryable<TEntity> queryEntity = null)
            where TEntity : class, new()
            where TSearchModel : ISearchModel
        {
            IQueryable<TEntity> queryList = GetQueryableList<TEntity, TSearchModel>(searchModel, queryEntity);
            return PageHelper.GetPageData(queryList, orderByKeySelector, pageParam, bIsAscOrder, searchModel?.Sort);
        }

        public virtual TEntity GetById<TEntity>(int id, DbSet<TEntity> entities = null, bool bIsNotNull = true)
          where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            Expression<Func<TEntity, bool>> predicate = PageHelper.GetPredicateById<TEntity>(id);
            TEntity entity = entities.FirstOrDefault(predicate);
            if (bIsNotNull && entity == null)
            {
                throw new Exception("Not exist [" + typeof(TEntity).Name + "] entity with Id=" + id);
            }
            return entity;
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(int id, DbSet<TEntity> entities = null, bool bIsNotNull = true)
             where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            Expression<Func<TEntity, bool>> predicate = PageHelper.GetPredicateById<TEntity>(id);
            TEntity entity = await entities.FirstOrDefaultAsync(predicate);
            if (bIsNotNull && entity == null)
            {
                throw new Exception("Not exist [" + typeof(TEntity).Name + "] entity with Id=" + id);
            }
            return entity;
        }

        public virtual List<TEntity> GetByIdList<TEntity>(List<int> idList, DbSet<TEntity> entities = null)
            where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            Expression<Func<TEntity, bool>> predicate = PageHelper.GetPredicateByIdList<TEntity>(idList);
            List<TEntity> entityList = entities.Where(predicate).ToList();
            return entityList;
        }

        public async Task<bool> DeleteAsync<TEntity>(int id, DbSet<TEntity> entities = null)
            where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            TEntity entity = await GetByIdAsync<TEntity>(id);
            entities.Remove(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync<TEntity>(TEntity entity, DbSet<TEntity> entities = null)
         where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            Attach(entities, entity, EntityState.Deleted);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteListAsync<TEntity>(List<int> idList, DbSet<TEntity> entities = null)
            where TEntity : class, new()
        {
            entities = entities ?? GetDbSet<TEntity>();
            List<TEntity> entityList = GetByIdList<TEntity>(idList);
            entities.RemoveRange(entityList);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteListAsync<TEntity>(List<TEntity> entityList, DbSet<TEntity> entities = null)
           where TEntity : class, new()
        {
            foreach (TEntity entity in entityList)
            {
                Attach(entities, entity, EntityState.Deleted);
            }
            return await SaveChangesAsync();
        }

        protected void CheckNullEntity<TEntity>(TEntity entity)
            where TEntity : class, new()
        {
            if (entity == null)
            {
                throw new Exception("[" + typeof(TEntity).Name + "] entity can not be null!");
            }
        }

        public async Task<bool> SaveAsync<TEntity>(TEntity entity, DbSet<TEntity> entities = null)
             where TEntity : class, new()
        {
            CheckNullEntity(entity);

            entities = entities ?? GetDbSet<TEntity>();

            if (PageHelper.IsAdd<TEntity, int>(entity))
            {
                entities.Add(entity);
            }
            else
            {
                Attach(entities, entity, EntityState.Modified);
            }
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _Db.SaveChangesAsync()) > 0;
        }

        #endregion ORM

        #region Dispose

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }

        #endregion Dispose
    }

    public class DbContextBaseHelper<TDbContext, TEntity, TQueryableEntity> : DbContextBaseHelper<TDbContext>
        where TDbContext : DbContext, new()
        where TEntity : class, new()
        where TQueryableEntity : class, new()
    {
        #region private Fields

        private DbSet<TEntity> _entities;
        private DbSet<TEntity> _Entites => _entities ?? (_entities = GetDbSet());

        private bool? _isSameEntity;
        private bool _IsSameEntity => _isSameEntity ?? (_isSameEntity = typeof(TEntity) == typeof(TQueryableEntity)).Value;

        #endregion private Fields

        #region private functions

        public DbSet<TEntity> GetEntities()
        {
            return _Entites;
        }

        public DbSet<TEntity> GetDbSet()
        {
            return GetDbSet<TEntity>();
        }

        protected override void CheckEntityIfDiffThenMustOverride()
        {
            if (!_IsSameEntity)
            {
                throw new Exception("This method must be override!");
            }
        }

        #endregion private functions

        #region protected functions

        public virtual IQueryable<TQueryableEntity> GetQueryableList<TSearchModel>(TSearchModel seachModel)
            where TSearchModel : ISearchModel
        {
            CheckEntityIfDiffThenMustOverride();

            return _Entites as IQueryable<TQueryableEntity>;
        }

        #endregion protected functions

        #region public functions

        public virtual List<TQueryableEntity> GetList<TSearchModel>(TSearchModel seachModel)
              where TSearchModel : ISearchModel
        {
            return GetList<TQueryableEntity, TSearchModel>(seachModel, GetQueryableList<TSearchModel>(seachModel));
        }

        public virtual PageData<TQueryableEntity> GetPageList<TSearchModel>(TSearchModel seachModel, PageParam pageParam, bool bIsAscOrder = true)
              where TSearchModel : ISearchModel
        {
            return GetPageList<TQueryableEntity, TSearchModel>(seachModel, pageParam, bIsAscOrder, GetQueryableList<TSearchModel>(seachModel));
        }

        public virtual PageData<TQueryableEntity> GetPageList<TKey, TSearchModel>(TSearchModel seachModel, Expression<Func<TQueryableEntity, TKey>> orderByKeySelector, PageParam pageParam, bool bIsAscOrder = true)
              where TSearchModel : ISearchModel
        {
            return GetPageList<TQueryableEntity, TKey, TSearchModel>(seachModel, orderByKeySelector, pageParam, bIsAscOrder, GetQueryableList<TSearchModel>(seachModel));
        }

        public virtual Task<TEntity> GetById(int id, bool bIsNotNull = true)
        {
            return GetByIdAsync<TEntity>(id, null, bIsNotNull);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync<TEntity>(id);
        }

        public virtual async Task<bool> DeleteListAsync(List<int> idList)
        {
            return await DeleteListAsync<TEntity>(idList);
        }

        public void CheckNullEntity(TEntity entity)
        {
            if (entity == null)
            {
                throw new Exception("[" + typeof(TEntity).Name + "] entity can not be null!");
            }
        }

        public virtual async Task<bool> SaveAsync(TEntity entity)
        {
            return await SaveAsync<TEntity>(entity);
        }

        #endregion public functions
    }

    public class DbContextBaseHelper<TDbContext, TEntity> : DbContextBaseHelper<TDbContext, TEntity, TEntity>
        where TDbContext : DbContext, new()
        where TEntity : class, new()
    {
    }
}