using System;
using System.Threading.Tasks;

namespace Multi.Language.Infrastructure.Redis
{
    public class SessionManager<T> : SessionManagerBase
    {
        private readonly IRedisManager _redisManager;

        public SessionManager(IRedisManager redisManager)
        {
            _redisManager = redisManager;
        }



        public T this[string key]
        {
            get => GetSession(key);
            set => SetSession(key, value);
        }

        public T GetSession(string key)
        {
            return _redisManager.Get<T>(key, ExpirationTime);
        }

        public bool SetSession(string key, T data)
        {
            return _redisManager.Set(key, data, ExpirationTime);
        }

        public bool DeleteSession(string prefix, string sessionId)
        {
            var pattern = prefix + sessionId + "*";
            return _redisManager.DeleteByPattern(pattern);
        }

        public async Task<T> GetSessionAsync(string key)
        {
            return await _redisManager.GetAsync<T>(key, ExpirationTime);
        }

        public async Task<bool> SetSessionAsync(string key, T data)
        {
            return await _redisManager.SetAsync(key, data, ExpirationTime);
        }

        public async Task<bool> DeleteSessionAsync(string prefix, string sessionId)
        {
            var pattern = prefix + sessionId + "*";
            return await _redisManager.DeleteByPatternAsync(pattern);
        }
    }

    public class SessionManagerBase
    {
        public static TimeSpan ExpirationTime { get; set; }

        public static bool Delete(string pattern)
        {
            var redis = new RedisManager();
            return redis.DeleteByPattern(pattern);
        }
    }
}
