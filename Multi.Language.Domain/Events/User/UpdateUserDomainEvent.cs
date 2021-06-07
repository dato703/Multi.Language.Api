using System;
using App.Core;

namespace Multi.Language.Domain.Events.User
{
    public class UpdateUserDomainEvent : DomainEvent
    {
        public UpdateUserDomainEvent(Guid userId, string password, string email)
        {
            Password = password;
            Email = email;
            Id = userId;
        }

        public Guid Id { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
    }
}