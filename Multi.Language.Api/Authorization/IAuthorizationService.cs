using System;
using System.Collections.Generic;
using Multi.Language.Domain.UserAggregate;

namespace Multi.Language.Api.Authorization
{
    public interface IAuthorizationService
    {
        bool IsAuthorized { get; }
        AuthorizationUser CurrentUser { get; }
        Guid CurrentUserId { get; }
        string IpAddress { get; set; }
        string SessionId { get; set; }
        bool HasAnyRole(IList<UserRole> userRoles);
        string Login(AuthorizationUser user);
        bool LogOut(string sessionId);
        bool SaveVerifyCode(string code);
        bool CheckVerifyCode(string code);
        bool SetVerifyToken(string token, dynamic data);
        (bool, dynamic) CheckVerifyToken(string token);
    }
}
