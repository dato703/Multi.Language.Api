using System;
using System.Threading.Tasks;
using App.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multi.Language.Api.Authorization;
using Multi.Language.Application.Commands;
using Multi.Language.Application.Queries;
using Multi.Language.Application.ViewModels.User;
using Multi.Language.Application.Authorization;
using Multi.Language.Application.Commands.User;
using Multi.Language.Domain.UserAggregate;

namespace Multi.Language.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : BaseController
    {

        public AccountController(CommandProcessor commandProcessor, QueryProcessor queryProcessor, IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor) : base(commandProcessor, queryProcessor, authorizationService, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("is-authorized")]
        public IActionResult IsAuthorized()
        {
            try
            {
                if (!Request.Headers.TryGetValue("session-id", out var values))
                {
                    return Ok(new HttpResult { Status = HttpResultStatus.Unauthorized });
                }

                AuthorizationService.SessionId = values[0];
                var result = new HttpResult()
                {
                    Status = AuthorizationService.IsAuthorized ? HttpResultStatus.Success : HttpResultStatus.Unauthorized
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var command = new LoginCommand(model);
            await CommandProcessor.Execute(command);
            return Ok(command.HttpResult);
        }

        [HttpPost]
        [AuthorizedUserRole(UserRole.User, UserRole.Administrator, UserRole.Administrator)]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            if (!Request.Headers.TryGetValue("session-id", out var values))
            {
                return Ok(new HttpResult { Status = HttpResultStatus.Unauthorized });
            }

            var sessionId = values[0];
            if (string.IsNullOrEmpty(sessionId))
            {
                return Ok(new HttpResult { Status = HttpResultStatus.Unauthorized });
            }

            var logoutCommand = new LogoutCommand(sessionId);
            await CommandProcessor.Execute(logoutCommand);

            return Ok(logoutCommand.HttpResult);
        }
    }
}
