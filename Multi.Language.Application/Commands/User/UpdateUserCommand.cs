using System;
using MediatR;

namespace Multi.Language.Application.Commands.User
{
    public class UpdateUserCommand : IRequest
    {
        public UpdateUserCommand(Guid userId, string password, string email)
        {
            Password = password;
            Email = email;
            UserId = userId;
        }

        public Guid UserId { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
    }
}
