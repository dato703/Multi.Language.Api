using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Multi.Language.Infrastructure.Authorization;

namespace Multi.Language.Application.Commands.Account
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly IAuthorizationService _authorizationService;

        public LogoutCommandHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return await _authorizationService.LogOutAsync(request.SessionId);
        }
    }
}