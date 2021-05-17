﻿using System;
using System.Threading.Tasks;
using App.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Multi.Language.Api.Authorization;
using Multi.Language.Application;
using Multi.Language.Application.Authorization;
using Multi.Language.Application.Commands.Account;
using Multi.Language.Domain.UserAggregate;

namespace Multi.Language.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;
        private readonly RequestProcessor _requestProcessor;

        public AccountController(IMediator mediator,IAuthorizationService authorizationService, RequestProcessor requestProcessor)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
            _requestProcessor = requestProcessor;
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

                _authorizationService.SessionId = values[0];
                var result = new HttpResult()
                {
                    Status = _authorizationService.IsAuthorized ? HttpResultStatus.Success : HttpResultStatus.Unauthorized
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
            var result = _requestProcessor.Execute(command);
            //var result=await _mediator.Send(command);
            return Ok(_requestProcessor.HttpResult);
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
            var result = await _mediator.Send(logoutCommand);

            return Ok(result);
        }
    }
}
