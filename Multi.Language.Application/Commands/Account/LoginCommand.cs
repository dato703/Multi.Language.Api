using App.Core.Shared;
using MediatR;
using Newtonsoft.Json;

namespace Multi.Language.Application.Commands.Account
{
    public class LoginCommand : IRequest<object>
    {
        [JsonProperty("email")] public string Email { get; private set; }
        [JsonProperty("password")] public string Password { get; private set; }
        [JsonProperty("browser")] public BrowserInfo Browser { get; private set; }
    }
}
