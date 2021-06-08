using System.Threading;
using System.Threading.Tasks;
using App.Core;
using MediatR;
using Multi.Language.Domain.SeedWork;

namespace Multi.Language.Application.Commands.User
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.FindAsync(x => x.Id == request.UserId);
            if (user == null)
            {
                throw new DomainException("User Not found", ExceptionLevel.Error);
            }

            user.Update(request.Password, request.Email);

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}