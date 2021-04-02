using System.Threading.Tasks;
using App.Core;
using Multi.Language.Application.Authorization;
using Multi.Language.Application.ViewModels.User;

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
                UserRole = user.UserRole,
                LocalIpAddress = "",
            };

            var sessionId =AuthorizationService.Login(authorizationUser);

            HttpResult = string.IsNullOrEmpty(sessionId) ? HttpResult.Error() : HttpResult.Successful();
        }
    }
}
