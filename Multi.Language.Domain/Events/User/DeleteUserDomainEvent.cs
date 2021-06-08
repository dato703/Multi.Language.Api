using System;
using App.Core;

namespace Multi.Language.Domain.Events.User
{
    public class DeleteUserDomainEvent : DomainEvent
    {
        public DeleteUserDomainEvent(Guid id) : base(id)
        {
            Id = id;
        }
        public Guid Id { get; private set; }
    }
}