using MediatR;

namespace Multi.Language.Application.Commands.Account
{
    public class LogoutCommand : IRequest<bool>
    {
        public LogoutCommand(string sessionId)
        {
            SessionId = sessionId;
        }

        public string SessionId { get; private set; }
    }
}
