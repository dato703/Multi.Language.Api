using System;
using App.Core;

namespace Multi.Language.Domain.UserAggregate
{
    public class User : AggregateRoot<Guid>
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
        }
        public void Update(string password, string email)
        {
            Password = password;
            Email = email;
        }

    }
}
