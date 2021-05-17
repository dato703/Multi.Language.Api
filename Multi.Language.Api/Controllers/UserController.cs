using System;
using System.Threading.Tasks;
using App.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Multi.Language.Api.Authorization;
using Multi.Language.Application.Commands.User;
using Multi.Language.Application.Queries.User;
using Multi.Language.Domain.UserAggregate;

namespace Multi.Language.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("all")]
        [AuthorizedUserRole(UserRole.User, UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetUsersQuery();
            var users = await _mediator.Send(query);
            var result = new HttpResult("users", users);
            return Ok(result);
        }

        [HttpGet]
        [Route("details/{id}")]
        [AuthorizedUserRole(UserRole.User, UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> Details(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var user = await _mediator.Send(query);
            return Ok(new HttpResult("user", user));
        }

        [HttpPost]
        [Route("create")]
        [AuthorizedUserRole(UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var userId = await _mediator.Send(command);
            return Ok(userId);
        }

        [HttpGet]
        [Route("user-fro-update/{id}")]
        [AuthorizedUserRole(UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> GetUserForUpdate(Guid id)
        {
            var query = new GetUserForUpdateQuery(id);
            var user = await _mediator.Send(query);
            return Ok(new HttpResult("user", user));
        }


        [HttpPost]
        [Route("update")]
        [AuthorizedUserRole(UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route("delete")]
        [AuthorizedUserRole(UserRole.Administrator)]
        public async Task<IActionResult> DeleteConfirmed([FromBody] Guid userId)
        {
            var command = new DeleteUserCommand(userId);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
