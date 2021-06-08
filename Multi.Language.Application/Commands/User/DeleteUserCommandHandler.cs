using System.Threading;
using System.Threading.Tasks;
using App.Core;
using MediatR;
using Multi.Language.Domain.SeedWork;

namespace Multi.Language.Application.Commands.User
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.FindAsync(x => x.Id == request.UserId);
            if (user == null)
            {
                throw new DomainException("User Not found", ExceptionLevel.Error);
            }

            _unitOfWork.UserRepository.Remove(user);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}