using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Multi.Language.Domain.AggregatesModel.UserAggregate;
using Multi.Language.Domain.SeedWork;

namespace Multi.Language.Application.Commands.User
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Domain.AggregatesModel.UserAggregate.User();
            user.Create(request.UserName, request.Password, request.Email, UserRole.User);
            //user.Validate();
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();
            return user.Id;
        }
    }
}
