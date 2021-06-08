using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Multi.Language.Api.Helpers;
using Multi.Language.Application;
using Multi.Language.Infrastructure.Authorization;

namespace Multi.Language.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly RequestProcessor RequestProcessor;
        protected readonly IAuthorizationService AuthorizationService;
        protected readonly IHttpContextAccessor HttpContextAccessor;
        public BaseController(RequestProcessor requestProcessor, IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
        {
            RequestProcessor = requestProcessor;
            HttpContextAccessor = httpContextAccessor;
            AuthorizationService = authorizationService;
            AuthorizationService.SessionId = SessionId;
            AuthorizationService.IpAddress = IpAddress;
        }
        protected string IpAddress => GetClientIpAddress();
        private string SessionId
        {
            get
            {
                StringValues values = default;
                HttpContextAccessor?.HttpContext?.Request.Headers.TryGetValue("session-id", out values);
                if (values.Count == 0)
                {
                    return null;
                }
                var sessionId = values[0];
                return sessionId;
            }
        }

        protected string GetClientIpAddress()
        {
            
            //var ipAddress1= Request.HttpContext.Connection.RemoteIpAddress;

            var ipAddress = HttpContextAccessor.GetRequestIP();

            if (SessionId != null && AuthorizationService?.CurrentUser?.LocalIpAddress != null && IpHelper.ValidateIPv4(AuthorizationService?.CurrentUser?.LocalIpAddress) && ipAddress.ToLower().Trim().GetHashCode() != AuthorizationService?.CurrentUser?.LocalIpAddress?.ToLower().Trim().GetHashCode())
            {
                ipAddress += $",{AuthorizationService?.CurrentUser?.LocalIpAddress}";
            }
            return ipAddress.Trim();

        }
    }
}
