using System.Collections.Generic;
using App.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Multi.Language.Domain.UserAggregate;

namespace Multi.Language.Api.Authorization
{
    public class AuthorizedUserRoleAttribute : TypeFilterAttribute
    {
        public AuthorizedUserRoleAttribute(params UserRole[] roles) : base(typeof(AuthorizationUserRoleAttribute))
        {
            Arguments = new object[] { roles };
        }

        private class AuthorizationUserRoleAttribute : IActionFilter
        {
            private readonly IAuthorizationService _authorizationService;
            private readonly List<UserRole> _roles;

            public AuthorizationUserRoleAttribute(IAuthorizationService authorizationService, params UserRole[] roles)
            {
                _roles = new List<UserRole>();
                _roles.AddRange(roles);
                _authorizationService = authorizationService;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                _authorizationService.SessionId = context.HttpContext.Request.Headers["session-id"];

                if (!_authorizationService.IsAuthorized)
                {
                    var httpResult = HttpResult.Unauthorized();
                    context.Result = new OkObjectResult(httpResult);
                }
                else
                {
                    var hasRoles = _authorizationService.HasAnyRole(_roles);
                    if (!hasRoles)
                    {
                        var httpResult = HttpResult.AccessDenied();
                        context.Result = new OkObjectResult(httpResult);
                    }
                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }
        }
    }
}
