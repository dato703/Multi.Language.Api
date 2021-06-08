using System;
using System.Threading;
using System.Threading.Tasks;
using App.Core;
using MediatR;
using Multi.Language.Domain.SeedWork;
using Multi.Language.Infrastructure.Authorization;

namespace Multi.Language.Application.Commands.Account
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorizationService _authorizationService;

        public LoginCommandHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
        {
            _unitOfWork = unitOfWork;
            _authorizationService = authorizationService;
        }

        public async Task<object> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                throw new DomainException("იმეილი აუცილებელია", ExceptionLevel.Error);
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                throw new DomainException("პაროლი აუცილებელია", ExceptionLevel.Error);
            }

            var user = await _unitOfWork.UserRepository.FindAsync(x =>
                x.Password == request.Password && x.Email == request.Email);

            if (user == null)
            {
                throw new DomainException("მომხმარებელი ვერ მოიძებნა", ExceptionLevel.Warning);
            }

            var authorizationUser = new AuthorizationUser
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                UserRole = user.UserRole,
                LocalIpAddress = request.Browser.Ips.Count > 0 ? request.Browser.Ips[0] : "",
            };

            var sessionId = await _authorizationService.LoginAsync(authorizationUser);

            if (request.Browser.BrowserId == null || request.Browser.BrowserId == Guid.Empty)
            {
                request.Browser.BrowserId = Guid.NewGuid();
            }

            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                return new
                {
                    SessionId = sessionId,
                    LoginName = authorizationUser.UserName,
                    browserId = request.Browser.BrowserId,
                    AuthorizedUser = new
                    {
                        authorizationUser.Id,
                        authorizationUser.Email,
                        authorizationUser.UserName,
                        authorizationUser.UserRole,
                    }
                };
            }

            return null;
        }
    }
}