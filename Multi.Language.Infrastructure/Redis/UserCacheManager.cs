using System;

namespace Multi.Language.Infrastructure.Redis
{
    public class UserCacheManagerBase
    {
        public static TimeSpan ExpirationTime { get; set; } = new TimeSpan(0, 0, 10);
    }

    public class UserCacheManager<T> : UserCacheManagerBase
    {
        private readonly IRedisManager _redisManager;
        public bool CacheEnabled => _redisManager != null;
        public UserCacheManager(IRedisManager redisManager)
        {
            _redisManager = redisManager;
        }


        public T this[string key]
        {
            get => GetUsers(key);
            set => SetUsers(key, value);
        }

        private T GetUsers(string key)
        {
            return _redisManager.Get<T>(key, ExpirationTime);
        }

        private bool SetUsers(string key, T data)
        {
            return _redisManager.Set(key, data, ExpirationTime);
        }

        public bool DeleteUsers(string key)
        {
            return _redisManager.Delete(key);
        }
    }
}
