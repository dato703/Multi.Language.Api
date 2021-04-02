using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Multi.Language.Api.Authorization;
using Multi.Language.Api.Configuration;
using Multi.Language.Application.Commands;
using Multi.Language.Application.Queries;
using Multi.Language.Domain.SeedWork;
using Multi.Language.Domain.UserAggregate;
using Multi.Language.Infrastructure;
using Multi.Language.Infrastructure.Redis;
using Multi.Language.Infrastructure.Repositories;

namespace Multi.Language.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerMiddleWear();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRedisManager, RedisManager>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<CommandProcessor>();
            services.AddScoped<QueryProcessor>();
            services.AddRedis(Configuration);
            services.AddDatabase(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerMiddleWear();
            }

            app.UseCors(c => c
                .AllowAnyOrigin()
                .WithOrigins("http://localhost:4200")
                .WithExposedHeaders("unauthorized", "access-denied")
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .AllowCredentials()
                .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
