using System;
using System.Threading.Tasks;
using App.Core;
using Multi.Language.Application.Authorization;
using Multi.Language.Application.ViewModels.User;
using Newtonsoft.Json;

namespace Multi.Language.Application.Commands.User
{
    public class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _loginViewModel;

        public LoginCommand(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
        }
        internal override async Task Execute()
        {
            if (string.IsNullOrEmpty(_loginViewModel.Email))
            {
                throw new DomainException("იმეილი აუცილებელია", ExceptionLevel.Error);
            }

            if (string.IsNullOrEmpty(_loginViewModel.Password))
            {
                throw new DomainException("პაროლი აუცილებელია", ExceptionLevel.Error);
            }

            var user = await UnitOfWork.UserRepository.FindAsync(x => x.Password == _loginViewModel.Password && x.Email == _loginViewModel.Email);

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
                LocalIpAddress = _loginViewModel.Browser.Ips.Count > 0 ? _loginViewModel.Browser.Ips[0] : "",
            };

            var sessionId = await AuthorizationService.LoginAsync(authorizationUser);

            if (_loginViewModel.Browser.BrowserId == null || _loginViewModel.Browser.BrowserId == Guid.Empty)
            {
                _loginViewModel.Browser.BrowserId = Guid.NewGuid();
            }

            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                HttpResult.Parameters.Add("session-id", sessionId);
                HttpResult.Parameters.Add("login-name", authorizationUser.UserName);
                HttpResult.Parameters.Add("browser-id", _loginViewModel.Browser.BrowserId);
                HttpResult.Parameters.Add("authorized-user", JsonConvert.SerializeObject(new
                {
                    authorizationUser.Id,
                    authorizationUser.Email,
                    authorizationUser.UserName,
                    authorizationUser.UserRole,
                }));
            }

            HttpResult = string.IsNullOrEmpty(sessionId) ? HttpResult.Error() : HttpResult.Successful();
        }
    }
}
