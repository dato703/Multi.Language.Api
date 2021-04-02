using Microsoft.AspNetCore.Mvc;
using Multi.Language.Application.Commands;
using Multi.Language.Application.Queries;
using Multi.Language.Application.Authorization;

namespace Multi.Language.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly CommandProcessor CommandProcessor;
        protected readonly QueryProcessor QueryProcessor;
        protected readonly IAuthorizationService AuthorizationService;
        public BaseController(CommandProcessor commandProcessor, QueryProcessor queryProcessor, IAuthorizationService authorizationService)
        {
            CommandProcessor = commandProcessor;
            QueryProcessor = queryProcessor;
            AuthorizationService = authorizationService;
        }
    }
}
