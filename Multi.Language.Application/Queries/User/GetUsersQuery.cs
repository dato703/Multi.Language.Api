using System.Collections.Generic;
using MediatR;
using Multi.Language.Application.ViewModels.User;

namespace Multi.Language.Application.Queries.User
{
    public class GetUsersQuery : IRequest<List<UserViewModel>>
    {
    }
}
