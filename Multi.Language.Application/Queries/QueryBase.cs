using System;
using System.Threading.Tasks;
using App.Core;
using Multi.Language.Domain.SeedWork;

namespace Multi.Language.Application.Queries
{
    public abstract class QueryBase<T> : IDisposable
    {
        protected IUnitOfWork UnitOfWork;

        protected QueryBase()
        {
            HttpResult = new HttpResult();
        }
        public string IpAddress { get; set; }
        public HttpResult HttpResult { get; internal set; }
        public void Initialize(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        internal abstract Task<T> Execute();

        public void Dispose()
        {

        }
    }
}
