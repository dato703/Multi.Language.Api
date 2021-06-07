using System;
using App.Core;
using Multi.Language.Domain.Events.User;

namespace Multi.Language.Domain.AggregatesModel.UserAggregate
{
    public class User : Entity<Guid>, IAggregateRoot
    {
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public UserRole UserRole { get; private set; }

        public void Create(string userName, string password, string email, UserRole userRole)
        {
            UserName = userName;
            Password = password;
            Email = email;
            UserRole = userRole;
            AddDomainEvent(new CreateUserDomainEvent(this));
        }
        public void Update(string password, string email)
        {
            Password = password;
            Email = email;
            AddDomainEvent(new UpdateUserDomainEvent(Id, password, email));
        }

    }
}
