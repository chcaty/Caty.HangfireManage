using Caty.ContextMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Repositorys
{
    /// <summary>
    /// ef通用数据处理
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        /// <summary>
        /// 通过filter获取指定条件下所有数据
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IList<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// 通过filter获取指定条件分页查询数据
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        Task<IList<TEntity>> GetPage(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int pageSize = 20,
            int pageIndex = 1,
            params Expression<Func<TEntity, object>>[] includes
           );

        /// <summary>
        /// 通过指定条件获取数据（建议使用索引列）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 通过制定条件获取数据（分页）
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="expression"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        Task<IList<TEntity>> List<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, int pageSize, int pageIndex, bool isAsc);

        /// <summary>
        /// 通过指定条件获取数据（慎用）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IList<TEntity>> List(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// SQL语句筛选列表
        /// </summary>
        /// <param name="sql">查询表为当期实体表，字段为*的sql语句</param>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IList<TEntity>> List(string sql, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 多OrderBy查询封装
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="orderBys"></param>
        /// <returns></returns>
        Task<IList<TEntity>> List(Expression<Func<TEntity, bool>> spec, int pageSize, int pageIndex,
            params Tuple<Expression<Func<TEntity, object>>, bool>[] orderBys);

        /// <summary>
        ///  通过制定条件获取数据的总数
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ValueTask<int> GetCount(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 获取指定数据
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<TEntity> One(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] propertyPaths);

        /// <summary>
        /// 查询指定数据
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<TEntity> One(Expression<Func<TEntity, bool>> spec, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        /// <summary>
        /// 根据条件查询指定数据并且进行排序
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 获取单个数据通过指定编号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ValueTask<TEntity> GetById(int id);

        /// <summary>
        /// 通过filter获取指定单个数据
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<TEntity> GetFirstOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// 添加单条数据
        /// </summary>
        /// <param name="entity"></param>
        Task Add(TEntity entity);

        /// <summary>
        /// 记录数据添加并且立即保存到数据库，如果此：DbContext启用了事务，那么受此事务影响
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        ValueTask<TEntity> AddSaveAsync(TEntity entity);

        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新指定数据
        /// </summary>
        /// <param name="entity"></param>
        Task Update(TEntity entity);

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entities">要更新的数据</param>
        /// <returns></returns>
        Task Update(IList<TEntity> entities);

        /// <summary>
        /// 删除指定数据
        /// </summary>
        /// <param name="id">唯一Key值</param>
        void Remove(int id);

        /// <summary>
        /// 移除指定对象
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task RemoveRange(IList<int> entities);

        /// <summary>
        ///  执行数据保存
        /// </summary>
        /// <returns>影响的数据集数量</returns>
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}