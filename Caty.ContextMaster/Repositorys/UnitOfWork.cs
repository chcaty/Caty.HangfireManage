using Caty.ContextMaster.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Repositorys
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable where TContext : DbContext, new()
    {
        private readonly TContext _context;

        private bool _disposed;
        private IDbContextTransaction _objTran;
        private IDictionary<string, object> _repositories;

        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        public TContext Context { get { return _context; } }

        public Task Commit(CancellationToken cancellationToken = default)
        {
            return _objTran?.CommitAsync(cancellationToken);
        }

        public async Task CreateTransaction(CancellationToken cancellationToken = default)
        {
            _objTran = await _context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ValueTask Rollback(CancellationToken cancellationToken = default)
        {
            _objTran?.RollbackAsync(cancellationToken);
            return _objTran.DisposeAsync();
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public TRepository GetCustomRepository<TRepository>()
        {
            var type = typeof(TRepository);

            if (_repositories.ContainsKey(type.Name))
                return (TRepository)_repositories[type.Name];

            if (type.GetInterfaces().All(i => !string.Equals($"{i.Namespace}.{i.Name}", "IGenericRepository", StringComparison.OrdinalIgnoreCase)))
                throw new Exception("自定义类型必须继承自IGenericRepository<TEntity>，且构造函数中仅有DbContext一个参数。");

            var repositoryInstance = Activator.CreateInstance(type, _context);

            if (repositoryInstance != null)
                _repositories.Add(type.Name, repositoryInstance);

            return (TRepository)repositoryInstance;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            _repositories ??= new Dictionary<string, object>();

            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
                return (IGenericRepository<TEntity>)_repositories[type];
            var repositoryType = typeof(GenericRepository<>);
            var typeArgs = new[] { typeof(TEntity) };
            var constructed = repositoryType.MakeGenericType(typeArgs);
            var repositoryInstance = Activator.CreateInstance(constructed, _context);

            if (repositoryInstance != null)
                _repositories.Add(type, repositoryInstance);
            return (IGenericRepository<TEntity>)_repositories[type];
        }
    }
}