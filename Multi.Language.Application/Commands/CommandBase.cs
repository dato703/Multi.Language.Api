﻿using System.Threading.Tasks;
using App.Core;
using Multi.Language.Domain.SeedWork;

namespace Multi.Language.Application.Commands
{
    public abstract class CommandBase
    {
        protected IUnitOfWork UnitOfWork;

        protected CommandBase()
        {
            HttpResult = new HttpResult();
        }
        public string IpAddress { get; set; }
        public HttpResult HttpResult { get; internal set; }

        public void Initialize(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public virtual bool NeedTransaction { get; set; } = false;
        internal abstract Task Execute();
    }
}
