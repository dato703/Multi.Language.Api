using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Multi.Language.Domain.UserAggregate;

namespace Multi.Language.Application.Authorization
{
    public interface IAuthorizationService
    {
        bool IsAuthorized { get; }
        AuthorizationUser CurrentUser { get; }
        Guid CurrentUserId { get; }
        string IpAddress { get; set; }
        string SessionId { get; set; }
        bool HasAnyRole(IList<UserRole> userRoles);
        Task<string> LoginAsync(AuthorizationUser user);
        Task<bool> LogOutAsync(string sessionId);
        bool SaveVerifyCode(string code);
        bool CheckVerifyCode(string code);
        bool SetVerifyToken(string token, dynamic data);
        (bool, dynamic) CheckVerifyToken(string token);
    }
}
