using App.Core.Shared;
using Newtonsoft.Json;

namespace Multi.Language.Application.ViewModels.User
{
    public class LoginViewModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("browser")]
        public BrowserInfo Browser { get; set; }
    }
}
