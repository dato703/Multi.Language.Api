using System;
using MediatR;

namespace Multi.Language.Application.Commands.User
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public CreateUserCommand(string userName, string password, string email)
        {
            UserName = userName;
            Password = password;
            Email = email;
        }

        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
    }
}
