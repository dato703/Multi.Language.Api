using System;
using System.Threading.Tasks;
using App.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multi.Language.Api.Authorization;
using Multi.Language.Application;
using Multi.Language.Application.Commands.Account;
using Multi.Language.Domain.AggregatesModel.UserAggregate;
using Multi.Language.Infrastructure.Authorization;

namespace Multi.Language.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : BaseController
    {

        public AccountController(RequestProcessor requestProcessor, IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor) : base(requestProcessor, authorizationService, httpContextAccessor)
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
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await RequestProcessor.Execute(command);
            RequestProcessor.HttpResult.AddParameter("login-data", result);
            return Ok(RequestProcessor.HttpResult);
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
            var result = await RequestProcessor.Execute(logoutCommand);

            return Ok(result);
        }
    }
}
