using System.Threading.Tasks;
using App.Core;

namespace Multi.Language.Application.Commands.User
{
    public class LogoutCommand : CommandBase
    {
        private readonly string _sessionId;

        public LogoutCommand(string sessionId)
        {
            _sessionId = sessionId;
        }
        internal override Task Execute()
        {
            HttpResult = AuthorizationService.LogOut(_sessionId) ? HttpResult.Successful() : HttpResult.Error();

            return Task.CompletedTask;
        }
    }
}
