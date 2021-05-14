using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Multi.Language.Application.ViewModels.User;
using Multi.Language.Domain.SeedWork;

namespace Multi.Language.Application.Queries.User
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<UserViewModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var result = users.Select(x => new UserViewModel
            {
                UserId = x.Id,
                UserName = x.UserName,
                Password = x.Password,
                Email = x.Email
            }).ToList();
            return result;
        }
    }
}