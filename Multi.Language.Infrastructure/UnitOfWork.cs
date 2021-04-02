using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Multi.Language.Domain.SeedWork;
using Multi.Language.Domain.UserAggregate;
using Multi.Language.Infrastructure.Repositories;

namespace Multi.Language.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        protected UserContext Context;
        public UnitOfWork(UserContext context)
        {
            Context = context;
        }
        public IUserRepository UserRepository => new UserRepository(Context);
        public async Task UseTransaction(Action action)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                action();
                await dbContextTransaction.CommitAsync();
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
                throw;
            }
        }

        public void DetachChanges()
        {
            var entries = Context.ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
        }

        public virtual async Task<int> CompleteAsync()
        {
            return await Context.SaveChangesAsync();
        }

        private bool _disposed;
        private readonly object _disposeLock = new object();
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            lock (_disposeLock)
            {
                if (_disposed) return;
                if (disposing)
                {
                    Context.Dispose();
                    Context = null;
                }
                _disposed = true;
            }
        }
    }
}
