using System;
using AutoMapper;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLemos.Api.Framework.Extensions.Startup;
using NLemos.Api.Identity.Data;
using NLemos.Api.Identity.Dto;
using NLemos.Api.Identity.Security;
using NLemos.Api.Identity.Services;

namespace NLemos.Api.Identity
{
    public class Startup
    {
        private string SwaggerTitle => Configuration.GetSection("Swagger").GetSection("Title").Value;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(RegisterIn).Assembly);

            var connectionString = Configuration.GetConnectionString("identity");

            services
                .AddSingleton(i => new IdentityContext(connectionString))
                .AddScoped<IIdentityRepository, IdentityRepository>()
                .AddScoped<IRegisterService, RegisterService>();

            services
                .AddScoped<IResourceOwnerPasswordValidator, PasswordValidator>()
                .AddScoped<IProfileService, ProfileService>();

            services
                .AddIdentityServer(o =>
                {
                    o.Authentication.CookieLifetime = new TimeSpan(360, 0, 0, 0);
                    o.Authentication.CookieSlidingExpiration = false;
                })
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryClients(Config.GetClients())
                .Services.AddTransient<ICorsPolicyService>(p =>
                {
                    var corsService = new DefaultCorsPolicyService(
                                        p.GetRequiredService<ILogger<DefaultCorsPolicyService>>()
                                    );
                    corsService.AllowAll = true;
                    return corsService;
                });

            services
                .AddBasicServices()
                .AddSwagger(SwaggerTitle);

            AddAuthentication(services);
        }

        private void AddAuthentication(IServiceCollection services)
        {
            var authUrl = (Configuration.GetSection("Auth")?.GetSection("Authority")?.Value ?? "Invalid Url");
            var audience = Configuration.GetSection("Auth")?.GetSection("ScopeName")?.Value ?? "services_auth";
            services.AddAuthentication(authUrl, audience);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseErrorHandling()
                .UseIdentityServer()
                .UseAuthentication()
                .ConfigureBasicApp()
                .ConfigureSwagger(SwaggerTitle);
        }
    }
}
