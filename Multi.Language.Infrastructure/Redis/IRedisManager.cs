using System;
using System.Threading.Tasks;

namespace Multi.Language.Infrastructure.Redis
{
    public interface IRedisManager : IDisposable
    {
        Task<bool> SetAsync(string key, object value);
        Task<bool> SetAsync(string key, object value, TimeSpan expirationTime);
        bool Delete(string key);
        bool DeleteByPattern(string pattern);
        T Get<T>(string key, TimeSpan expirationTime);
        T Get<T>(string key);
        bool Set(string key, object value, TimeSpan expirationTime);
        bool Set(string key, object value);
        Task<bool> DeleteByPatternAsync(string pattern);
        Task<bool> DeleteAsync(string key);
        Task<T> GetAsync<T>(string key, TimeSpan expirationTime);
        Task<T> GetAsync<T>(string key);

    }
}
