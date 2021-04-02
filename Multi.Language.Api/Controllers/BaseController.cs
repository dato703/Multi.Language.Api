using Microsoft.AspNetCore.Mvc;
using Multi.Language.Application.Commands;
using Multi.Language.Application.Queries;

namespace Multi.Language.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly CommandProcessor CommandProcessor;
        protected readonly QueryProcessor QueryProcessor;
        public BaseController(CommandProcessor commandProcessor, QueryProcessor queryProcessor)
        {
            CommandProcessor = commandProcessor;
            QueryProcessor = queryProcessor;
        }
    }
}
