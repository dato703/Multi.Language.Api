using System.Threading;
using System.Threading.Tasks;
using App.Core;
using MediatR;
using Multi.Language.Application.ViewModels.User;
using Multi.Language.Domain.SeedWork;

namespace Multi.Language.Application.Queries.User
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.FindAsync(x => x.Id == request.UserId);

            if (user == null)
            {
                throw new DomainException("User not found", ExceptionLevel.Error);
            }

            var result = new UserViewModel(user.Id, user.UserName, user.Password, user.Email);

            return result;
        }
    }
}