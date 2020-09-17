using Caty.ContextMaster.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Repositorys
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<TEntity>();
        }

        public virtual void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Remove(int id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual Task RemoveRange(IList<int> entities)
        {
            if (entities.Count > 200)
                throw new OverflowException("同时执行删除的数据超过200条");

            var results = from Q in _dbSet
                          where entities.Contains(Q.Id)
                          orderby Q.Id
                          select Q;

            _dbSet.RemoveRange(results);
            return Task.CompletedTask;
        }

        public virtual async Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<IList<TEntity>> GetPage(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int pageSize = 20, int pageIndex = 1, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = includes.Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(_dbSet, (current, include) => current.Include(include));

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return await query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>> spec)
        {
            return await _dbSet.Where(spec).OrderByDescending(u => u.Id).Take(200).ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<IList<TEntity>> List<TKey>(Expression<Func<TEntity, bool>> spec,
            Expression<Func<TEntity, TKey>> orderBy, int pageSize, int pageIndex, bool isAsc) =>
            isAsc
                ? await _dbSet.Where(spec).OrderBy(orderBy)
                    .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync()
.ConfigureAwait(false) : await _dbSet.Where(spec).OrderByDescending(orderBy)
                    .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync().ConfigureAwait(false);

        public virtual async Task<IList<TEntity>> List(Expression<Func<TEntity, bool>> spec) =>
            await _dbSet.Where(spec).ToListAsync().ConfigureAwait(false);

        public async Task<IList<TEntity>> List(string sql, Expression<Func<TEntity, bool>> expression) => await (string.IsNullOrEmpty(sql) ? _dbSet : _dbSet.FromSqlRaw(sql)).Where(expression).ToListAsync().ConfigureAwait(false);

        public virtual async Task<IList<TEntity>> List(Expression<Func<TEntity, bool>> spec, int pageSize, int pageIndex, params Tuple<Expression<Func<TEntity, object>>, bool>[] orderBys)
        {
            IQueryable<TEntity> entities = _dbSet.Where(spec);
            if (orderBys.Length >= 1)
            {
                bool isAsc = orderBys[0].Item2;
                Expression<Func<TEntity, object>> orderExpression = orderBys[0].Item1;
                entities = isAsc ? entities.OrderBy(orderExpression) : entities.OrderByDescending(orderExpression);
                var orderEntities = (IOrderedQueryable<TEntity>)entities;
                for (int i = 1; i < orderBys.Length; i++)
                {
                    bool isAsc1 = orderBys[i].Item2;
                    Expression<Func<TEntity, object>> orderExpression1 = orderBys[i].Item1;
                    orderEntities = isAsc1 ? orderEntities.ThenBy(orderExpression1) : orderEntities.ThenByDescending(orderExpression1);
                }
                entities = orderEntities;
            }
            return await entities.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync().ConfigureAwait(false);
        }

        public virtual async ValueTask<int> GetCount(Expression<Func<TEntity, bool>> spec) =>
            await _dbSet.Where(spec).CountAsync().ConfigureAwait(false);

        public virtual async Task<TEntity> One(Expression<Func<TEntity, bool>> spec, params Expression<Func<TEntity, object>>[] propertyPaths)
        {
            var queryable = _dbSet.Where(spec);
            foreach (var propertyPath in propertyPaths)
                queryable = queryable.Include(propertyPath);

            var listAsync = await queryable.Take(1).ToListAsync().ConfigureAwait(false);
            if (listAsync.Any())
                return listAsync.First();
            return null;
        }

        public virtual async Task<TEntity> One(Expression<Func<TEntity, bool>> spec, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            var listAsync = _dbSet.Where(spec).Take(1);
            if (orderBy != null)
                listAsync = orderBy(listAsync);
            if (listAsync.Any())
                return await listAsync.FirstOrDefaultAsync().ConfigureAwait(false);
            return null;
        }

        public virtual ValueTask<TEntity> GetById(int id)
        {
            return _dbSet.FindAsync(id);
        }

        public virtual Task<TEntity> GetFirstOrDefault(Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            return query.FirstOrDefaultAsync(filter);
        }

        public virtual Task Add(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Added;
            return Task.CompletedTask;
            //return _dbSet.AddAsync(entity).AsTask();
        }

        public virtual Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate);
        }

        /// <summary>
        /// 保存更乞
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual Task Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities">要更的数据列表</param>
        /// <returns></returns>
        public virtual Task Update(IList<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 即可保存当前值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async ValueTask<TEntity> AddSaveAsync(TEntity entity)
        {
            var result = _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return result.Result.Entity;
        }
    }
}