using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Multi.Language.Api.Configuration;
using Multi.Language.Domain.SeedWork;
using Multi.Language.Infrastructure;
using Multi.Language.Infrastructure.Redis;
using Multi.Language.Infrastructure.Repositories;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Multi.Language.Application;
using Multi.Language.Application.Configuration;
using Multi.Language.Domain.AggregatesModel.UserAggregate;
using Multi.Language.Infrastructure.Authorization;
using Multi.Language.Infrastructure.EventSourcing;

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
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerMiddleWear();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRedisManager, RedisManager>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<RequestProcessor>();
            services.AddRedis(Configuration);
            services.AddDatabase(Configuration);

            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new MediatorModule());
            return new AutofacServiceProvider(container.Build());
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
