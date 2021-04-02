using System;
using System.Threading.Tasks;
using Multi.Language.Domain.UserAggregate;

namespace Multi.Language.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository UserRepository { get; }
        Task UseTransaction(Action action);
        void DetachChanges();
        Task<int> CompleteAsync();
    }
}
