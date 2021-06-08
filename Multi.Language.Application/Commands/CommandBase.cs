using System.Threading.Tasks;
using App.Core;
using Multi.Language.Domain.SeedWork;
using Multi.Language.Infrastructure.Authorization;

namespace Multi.Language.Application.Commands
{
    public abstract class CommandBase
    {
        protected IUnitOfWork UnitOfWork;
        protected IAuthorizationService AuthorizationService;

        protected CommandBase()
        {
            HttpResult = new HttpResult();
        }
        public string IpAddress { get; set; }
        public HttpResult HttpResult { get; internal set; }

        public void Initialize(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
        {
            UnitOfWork = unitOfWork;
            AuthorizationService = authorizationService;
        }

        public virtual bool NeedTransaction { get; set; } = false;
        internal abstract Task Execute();
    }
}
