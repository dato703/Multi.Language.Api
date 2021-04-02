using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Multi.Language.Infrastructure.Redis;

namespace Multi.Language.Api.Configuration
{
    public static class RedisConfiguration
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            if (!int.TryParse(configuration.GetSection("Redis")["SessionTimeout"], out var timeout))
            {
                timeout = 30;
            }
            SessionManagerBase.ExpirationTime = TimeSpan.FromMinutes(timeout);
            if (!int.TryParse(configuration.GetSection("Redis")["UsersCacheTimeout"], out timeout))
            {
                timeout = 60;
            }
            UserCacheManagerBase.ExpirationTime = TimeSpan.FromMinutes(timeout);
            var redisConnection = configuration.GetSection("Redis")["Server"];
            if (redisConnection.Contains(";"))
            {
                var redisHosts = redisConnection.Split(";");
                RedisManager.Initialize(true, redisHosts);
            }
            else
            {
                RedisManager.Initialize(true, redisConnection);
            }
            return services;
        }
    }
}
