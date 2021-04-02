using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Multi.Language.Infrastructure;

namespace Multi.Language.Api.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserContext>(options => options.UseSqlServer(configuration.GetConnectionString("MultiLanguageDbConnectionString")));
            InitializeDatabase(services);
            return services;
        }

        private static void InitializeDatabase(IServiceCollection services)
        {
            var buildServiceProvider = services.BuildServiceProvider();
            if (!(buildServiceProvider.GetService(typeof(UserContext)) is UserContext referenceBookDbContext)) return;
            referenceBookDbContext.Database.EnsureCreated();
            referenceBookDbContext.Dispose();
        }
    }
}
