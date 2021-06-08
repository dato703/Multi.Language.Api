using System;
using System.Threading.Tasks;
using App.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multi.Language.Api.Authorization;
using Multi.Language.Application;
using Multi.Language.Application.Commands.User;
using Multi.Language.Application.Queries.User;
using Multi.Language.Domain.AggregatesModel.UserAggregate;
using Multi.Language.Infrastructure.Authorization;

namespace Multi.Language.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseController
    {

        public UserController(RequestProcessor requestProcessor, IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor) : base(requestProcessor, authorizationService, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("all")]
        [AuthorizedUserRole(UserRole.User, UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetUsersQuery();
            var users = await RequestProcessor.Execute(query);
            RequestProcessor.HttpResult.AddParameter("users", users);
            return Ok(RequestProcessor.HttpResult);
        }

        [HttpGet]
        [Route("details/{id}")]
        [AuthorizedUserRole(UserRole.User, UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> Details(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var user = await RequestProcessor.Execute(query);
            RequestProcessor.HttpResult.AddParameter("user", user);
            return Ok(RequestProcessor.HttpResult);
        }

        [HttpPost]
        [Route("create")]
        [AuthorizedUserRole(UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var userId = await RequestProcessor.Execute(command);
            RequestProcessor.HttpResult.AddParameter("userId", userId);
            return Ok(RequestProcessor.HttpResult);
        }

        [HttpGet]
        [Route("user-fro-update/{id}")]
        [AuthorizedUserRole(UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> GetUserForUpdate(Guid id)
        {
            var query = new GetUserForUpdateQuery(id);
            var user = await RequestProcessor.Execute(query);
            RequestProcessor.HttpResult.AddParameter("user", user);
            return Ok(RequestProcessor.HttpResult);
        }


        [HttpPost]
        [Route("update")]
        [AuthorizedUserRole(UserRole.Administrator, UserRole.SuperAdministrator)]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            await RequestProcessor.Execute(command);
            return Ok(RequestProcessor.HttpResult);
        }

        [HttpPost]
        [Route("delete")]
        [AuthorizedUserRole(UserRole.Administrator)]
        public async Task<IActionResult> DeleteConfirmed([FromBody] Guid userId)
        {
            var command = new DeleteUserCommand(userId);
            await RequestProcessor.Execute(command);
            return Ok(RequestProcessor.HttpResult);
        }
    }
}
