using App.Core.Shared;
using MediatR;
using Newtonsoft.Json;

namespace Multi.Language.Application.Commands.Account
{
    public class LoginCommand : IRequest<object>
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("browser")]
        public BrowserInfo Browser { get; set; }
    }
}
