using System;
using System.Threading.Tasks;
using App.Core;
using Microsoft.AspNetCore.Mvc;
using Multi.Language.Api.Authorization;
using Multi.Language.Application;
using Multi.Language.Application.Commands.User;
using Multi.Language.Application.Queries.User;
using Multi.Language.Domain.AggregatesModel.UserAggregate;

namespace Multi.Language.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly RequestProcessor _requestProcessor;

        public UserController(RequestProcessor requestProcessor)
        {
            _requestProcessor = requestProcessor;
        }

        [HttpGet]
        [Route("all")]
        [AuthorizedUserRole(UserRole.User, UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetUsersQuery();
            var users = await _requestProcessor.Execute(query);
            _requestProcessor.HttpResult.AddParameter("users", users);
            return Ok(_requestProcessor.HttpResult);
        }

        [HttpGet]
        [Route("details/{id}")]
        [AuthorizedUserRole(UserRole.User, UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> Details(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var user = await _requestProcessor.Execute(query);
            _requestProcessor.HttpResult.AddParameter("user", user);
            return Ok(_requestProcessor.HttpResult);
        }

        [HttpPost]
        [Route("create")]
        [AuthorizedUserRole(UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var userId = await _requestProcessor.Execute(command);
            _requestProcessor.HttpResult.AddParameter("userId", userId);
            return Ok(_requestProcessor.HttpResult);
        }

        [HttpGet]
        [Route("user-fro-update/{id}")]
        [AuthorizedUserRole(UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> GetUserForUpdate(Guid id)
        {
            var query = new GetUserForUpdateQuery(id);
            var user = await _requestProcessor.Execute(query);
            _requestProcessor.HttpResult.AddParameter("user", user);
            return Ok(_requestProcessor.HttpResult);
        }


        [HttpPost]
        [Route("update")]
        [AuthorizedUserRole(UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            await _requestProcessor.Execute(command);
            return Ok(_requestProcessor.HttpResult);
        }

        [HttpPost]
        [Route("delete")]
        [AuthorizedUserRole(UserRole.Administrator)]
        public async Task<IActionResult> DeleteConfirmed([FromBody] Guid userId)
        {
            var command = new DeleteUserCommand(userId);
            await _requestProcessor.Execute(command);
            return Ok(_requestProcessor.HttpResult);
        }
    }
}
