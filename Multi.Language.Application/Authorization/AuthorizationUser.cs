using System;
using System.Text.RegularExpressions;
using Multi.Language.Domain.UserAggregate;

namespace Multi.Language.Application.Authorization
{
    public class AuthorizationUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserRole UserRole { get; set; }
        public string LocalIpAddress { get; set; }

        public AuthorizationUser()
        {

        }

        public static bool IsEmpty(AuthorizationUser user)
        {
            var match = Regex.Match(user?.LocalIpAddress ?? "", @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
            return user == null || user.Id == Guid.Empty || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.LocalIpAddress); //|| !match.Success;
        }
    }
}
