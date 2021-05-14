using System;
using MediatR;
using Multi.Language.Application.ViewModels.User;

namespace Multi.Language.Application.Queries.User
{
    public class GetUserForUpdateQuery: IRequest<UpdateUserViewModel>
    {
        public GetUserForUpdateQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}
