using App.Core.Shared;
using MediatR;

namespace Multi.Language.Application.Commands.Account
{
    public class LoginCommand : IRequest<object>
    {
        public LoginCommand(string email, string password, BrowserInfo browser)
        {
            Email = email;
            Password = password;
            Browser = browser;
        }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public BrowserInfo Browser { get; private set; }
    }
}
