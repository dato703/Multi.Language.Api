using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;
using App.Core;

namespace Multi.Language.Infrastructure.Redis
{
    public class RedisManager : IRedisManager
    {
        private ConnectionMultiplexer Redis => _connection.Value;
        private static Lazy<ConnectionMultiplexer> _connection;

        private static bool _throwOnException;
        private static string _formattedHosts;

        static RedisManager()
        {
            _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_formattedHosts));

        }

        private static void ReConnect()
        {
            _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_formattedHosts));
        }


        public RedisManager()
        {

        }



        public static void Initialize(bool throwOnException = true, params string[] hosts)
        {
            _formattedHosts = string.Join(",", hosts);
            _throwOnException = throwOnException;

        }

        public async Task<bool> SetAsync(string key, object value)
        {
            try
            {
                var database = Redis.GetDatabase();
                var serializedValue = JsonService.Serialize(value);
                return await database.StringSetAsync(key, serializedValue);
            }
            catch (RedisConnectionException ex)
            {
                ReConnect();
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
            catch (Exception)
            {
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
        }

        public async Task<bool> SetAsync(string key, object value, TimeSpan expirationTime)
        {
            try
            {
                var database = Redis.GetDatabase();
                var serializedValue = JsonService.Serialize(value);

                return await database.StringSetAsync(key, serializedValue, expirationTime);
            }
            catch (RedisConnectionException ex)
            {
                ReConnect();
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
            catch (Exception ex)
            {
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var database = Redis.GetDatabase();
                var result = database.StringGetAsync(key);
                var task = Task.Factory.StartNew(() =>
                {
                    var value = result.Result;
                    if (value.IsNullOrEmpty || value == "[]")
                    {
                        return default(T);
                    }
                    return JsonService.Deserialize<T>(value);
                });
                return await task;
            }
            catch (RedisConnectionException ex)
            {
                ReConnect();
                if (_throwOnException)
                {
                    throw;
                }
                return default(T);
            }
            catch (Exception ex)
            {
                if (_throwOnException)
                {
                    throw;
                }
                return default(T);
            }
        }

        public async Task<T> GetAsync<T>(string key, TimeSpan expirationTime)
        {
            try
            {
                var database = Redis.GetDatabase();

                var result = database.StringGetAsync(key);
                await database.KeyExpireAsync(key, expirationTime);

                var value = result.Result;
                if (value.IsNullOrEmpty)
                {
                    return default(T);
                }
                return JsonService.Deserialize<T>(value);
            }
            catch (RedisConnectionException ex)
            {
                ReConnect();
                if (_throwOnException)
                {
                    throw;
                }
                return default(T);
            }
            catch (Exception ex)
            {
                if (_throwOnException)
                {
                    throw;
                }
                return default(T);
            }
        }

        public async Task<bool> DeleteAsync(string key)
        {
            try
            {
                var database = Redis.GetDatabase();
                return await database.KeyDeleteAsync(key);
            }
            catch (RedisConnectionException ex)
            {
                ReConnect();
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
            catch (Exception ex)
            {
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
        }

        public async Task<bool> DeleteByPatternAsync(string pattern)
        {
            try
            {
                var endpoints = Redis.GetEndPoints();
                var database = _connection.Value.GetDatabase();

                foreach (var endpoint in endpoints)
                {
                    try
                    {
                        var server = Redis.GetServer(endpoint);
                        var keys = server.Keys(pattern: pattern);
                        foreach (var key in keys)
                        {
                            return await database.KeyDeleteAsync(key);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (_throwOnException)
                        {
                            throw;
                        }
                    }

                }
                return true;
            }
            catch (RedisConnectionException ex)
            {
                ReConnect();
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
            catch (Exception ex)
            {
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
        }

        public bool Set(string key, object value)
        {
            try
            {
                var database = Redis.GetDatabase();
                var serializedValue = JsonService.Serialize(value);

                return database.StringSet(key, serializedValue);
            }
            catch (RedisConnectionException ex)
            {
                ReConnect();
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
            catch (Exception ex)
            {
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
        }

        public bool Set(string key, object value, TimeSpan expirationTime)
        {
            try
            {
                var database = Redis.GetDatabase();
                var serializedValue = JsonService.Serialize(value);

                return database.StringSet(key, serializedValue, expirationTime);
            }
            catch (RedisConnectionException ex)
            {
                ReConnect();
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
            catch (Exception ex)
            {
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
        }

        public T Get<T>(string key)
        {
            try
            {
                var database = Redis.GetDatabase();
                var value = database.StringGet(key);
                if (value.IsNullOrEmpty)
                {
                    return default(T);
                }
                return JsonService.Deserialize<T>(value);
            }
            catch (RedisConnectionException ex)
            {
                ReConnect();
                if (_throwOnException)
                {
                    throw;
                }
                return default(T);
            }
            catch (Exception ex)
            {
                if (_throwOnException)
                {
                    throw;
                }
                return default(T);
            }
        }

        public T Get<T>(string key, TimeSpan expirationTime)
        {
            try
            {
                IDatabase database = Redis.GetDatabase();

                var value = database.StringGet(key);
                database.KeyExpire(key, expirationTime);

                if (value.IsNullOrEmpty)
                {
                    return default(T);
                }
                return JsonService.Deserialize<T>(value);
            }
            catch (RedisConnectionException ex)
            {
                ReConnect();
                if (_throwOnException)
                {
                    throw;
                }
                return default(T);
            }
            catch (Exception ex)
            {
                if (_throwOnException)
                {
                    throw;
                }
                return default(T);
            }
        }

        public bool Delete(string key)
        {
            try
            {
                var database = Redis.GetDatabase();
                return database.KeyDelete(key);
            }
            catch (RedisConnectionException ex)
            {
                ReConnect();
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
            catch (Exception ex)
            {
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
        }

        public bool DeleteByPattern(string pattern)
        {
            try
            {
                var endpoints = Redis.GetEndPoints();
                var database = _connection.Value.GetDatabase();

                foreach (var endpoint in endpoints)
                {
                    try
                    {
                        var server = Redis.GetServer(endpoint);

                        var keys = server.Keys(pattern: pattern).ToArray();
                        if (keys.Length > 0)
                        {
                            database.KeyDelete(keys);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (_throwOnException)
                        {
                            throw;
                        }
                    }

                }
                return true;
            }
            catch (RedisConnectionException ex)
            {
                ReConnect();
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
            catch (Exception ex)
            {
                if (_throwOnException)
                {
                    throw;
                }
                return false;
            }
        }

        public void Dispose()
        {

        }
    }
}
