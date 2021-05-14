using System;
using MediatR;

namespace Multi.Language.Application.Commands.User
{
    public class DeleteUserCommand : IRequest
    {
        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }

    }
}
