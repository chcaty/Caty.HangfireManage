using Caty.ContextMaster.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Repositorys
{
    /// <summary>
    /// 数据操作工作单元
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IUnitOfWork<out TContext> : IDisposable where TContext : DbContext, new()
    {
        /// <summary>
        ///  要操作的DbContext
        /// </summary>
        TContext Context { get; }

        Task CreateTransaction(CancellationToken cancellationToken = default);

        Task Commit(CancellationToken cancellationToken = default);

        ValueTask Rollback(CancellationToken cancellationToken = default);

        Task<int> SaveAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///  创建工作单元中的应用DbSet
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();

        public TRepository GetCustomRepository<TRepository>();
    }
}