using System;
using System.Collections.Generic;
using System.Linq;
using App.Core.Utils;
using Multi.Language.Domain.UserAggregate;
using Multi.Language.Infrastructure.Redis;

namespace Multi.Language.Application.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private const string SessionIdPrefix = "user_session_id_";
        private const string SessionIdSalt = "TRR34K)#4j54hH=--*dd?UU=>>FF#22vOOIH";
        private AuthorizationUser _currentUser;
        private readonly SessionManager<AuthorizationUser> _redisManager;
        private readonly SessionManager<string> _smsVerificationSession;
        private readonly SessionManager<dynamic> _tokenVerificationSession;

        public AuthorizationService(IRedisManager redis)
        {
            _redisManager = new SessionManager<AuthorizationUser>(redis);
            _smsVerificationSession = new SessionManager<string>(redis);
            _tokenVerificationSession = new SessionManager<dynamic>(redis);
        }
        public bool IsAuthorized => CurrentUser != null;
        public Guid CurrentUserId => IsAuthorized ? CurrentUser.Id : Guid.Empty;
        public string IpAddress { get; set; }
        public string SessionId { get; set; }

        public AuthorizationUser CurrentUser
        {
            get
            {
                if (_currentUser != null)
                {
                    return _currentUser;
                }

                var key = $"{SessionIdPrefix}{SessionId}_user";
                _currentUser = GetUser(key);
                return _currentUser;
            }
        }
        public bool HasAnyRole(IList<UserRole> userRoles)
        {
            return userRoles.Any(i => i == CurrentUser.UserRole);
        }

        public string Login(AuthorizationUser user)
        {
            if (user == null)
            {
                return "";
            }
            SessionId = GenerateSessionId();
            var key = $"{SessionIdPrefix}{SessionId}_user";

            SetUser(key, user);
            if (AuthorizationUser.IsEmpty(GetUser(key)))
            {
                return "";
            }
            return SessionId;
        }

        public bool LogOut(string sessionId)
        {
            return _redisManager.DeleteSession(SessionIdPrefix, sessionId);
        }

        public bool SaveVerifyCode(string smsCode)
        {
            if (string.IsNullOrWhiteSpace(smsCode))
            {
                return false;
            }
            var key = $"verify_sms_code_{SessionId}_user";
            var sha256 = DataGenerator.GenerateSha256($"{SessionIdSalt}_{smsCode}");

            return _smsVerificationSession.SetSession(key, sha256);
        }

        public bool CheckVerifyCode(string smsCode)
        {
            if (string.IsNullOrWhiteSpace(smsCode))
            {
                return false;
            }
            var key = $"verify_sms_code_{SessionId}_user";
            var sha256 = DataGenerator.GenerateSha256($"{SessionIdSalt}_{smsCode}");
            var getCode = _smsVerificationSession.GetSession(key);
            if (getCode == sha256)
            {
                _smsVerificationSession.DeleteSession("sms_", SessionId);
                return true;
            }

            return false;
        }

        public bool SetVerifyToken(string token, dynamic data)
        {
            if (string.IsNullOrWhiteSpace(token) || data == null)
            {
                return false;
            }
            var key = $"verify_token_{token}";

            return _tokenVerificationSession.SetSession(key, data);
        }

        public (bool, dynamic) CheckVerifyToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token) || token.Length != 64)
            {
                return (false, null);
            }
            var key = $"verify_token_{token}";
            var data = _tokenVerificationSession.GetSession(key);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            var json2 = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            if (json2?.Id != null && (json2.Id.ToString() != Guid.Empty.ToString()))
            {
                return (true, json2);
            }
            return (false, null);
        }

        private AuthorizationUser GetUser(string key)
        {
            var authorizationUser = _redisManager[key];
            return authorizationUser;
        }

        private void SetUser(string key, AuthorizationUser user)
        {
            _redisManager[key] = user;
        }

        private string GenerateSessionId()
        {
            return DataGenerator.GenerateSha256($"{SessionIdSalt}{Guid.NewGuid()}{SessionIdPrefix}{DateTime.Now:O}");
        }
    }
}
