using System;
using System.Linq;
using System.Threading.Tasks;
using App.Core;
using Microsoft.AspNetCore.Mvc;
using Multi.Language.Api.Authorization;
using Multi.Language.Application.Commands;
using Multi.Language.Application.Commands.User;
using Multi.Language.Application.Queries;
using Multi.Language.Application.Queries.User;
using Multi.Language.Application.ViewModels.User;
using Multi.Language.Domain.UserAggregate;

namespace Multi.Language.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseController
    {
        public UserController(CommandProcessor commandProcessor, QueryProcessor queryProcessor) : base(commandProcessor, queryProcessor)
        {
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetUsersQuery();
            var viewModel = await QueryProcessor.Execute(query);
            var result = new HttpResult("users", viewModel, query.HttpResult);
            return Ok(result);
        }

        [HttpGet]
        [Route("details/{id}")]
        [AuthorizedUserRole(UserRole.User, UserRole.Administrator)]
        public async Task<IActionResult> Details(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var viewModel = await QueryProcessor.Execute(query);
            return Ok(new HttpResult("user", viewModel, query.HttpResult));
        }

        [HttpPost]
        [Route("create")]
        [AuthorizedUserRole(UserRole.User, UserRole.Administrator)]
        public async Task<IActionResult> Create([FromBody] UserCreateViewModel viewModel)
        {
            var command = new CreateUserCommand(viewModel);
            await CommandProcessor.Execute(command);
            return Ok(command.HttpResult);
        }

        [HttpGet]
        [Route("user-fro-update/{id}")]
        [AuthorizedUserRole(UserRole.User, UserRole.Administrator)]
        public async Task<IActionResult> GetUserForUpdate(Guid id)
        {
            var query = new GetUserForUpdateQuery(id);
            var viewModel = await QueryProcessor.Execute(query);
            query.HttpResult.AddParameter("user", viewModel);
            return Ok(query.HttpResult);
        }


        [HttpPost]
        [Route("update")]
        [AuthorizedUserRole(UserRole.User, UserRole.Administrator)]
        public async Task<IActionResult> Update([FromBody] UpdateUserViewModel viewModel)
        {
            var command = new UpdateUserCommand(viewModel);
            await CommandProcessor.Execute(command);
            return Ok(command.HttpResult);
        }

        [HttpPost]
        [Route("delete")]
        [AuthorizedUserRole(UserRole.Administrator)]
        public async Task<IActionResult> DeleteConfirmed([FromBody] Guid userId)
        {
            var command = new DeleteUserCommand(userId);
            await CommandProcessor.Execute(command);
            return Ok(command.HttpResult);
        }
    }
}
