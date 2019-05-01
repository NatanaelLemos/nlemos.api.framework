using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace NLemos.Api.Framework.Extensions.Startup
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, string authorityUrl, string audience)
        {
            if (!authorityUrl.EndsWith("/"))
            {
                authorityUrl += "/";
            }

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authorityUrl;
                    options.Audience = audience;
                    options.RequireHttpsMetadata = false;
                });

            return services;
        }

        public static IServiceCollection AddTestAuthentication(this IServiceCollection services, string authorityUrl, string audience, Func<HttpMessageHandler> backchannelHttpHandlerFunc)
        {
            services
                .RemoveAuthentication()
                .AddAuthentication(o =>
                {
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.Authority = authorityUrl;
                    o.Audience = audience;
                    o.RequireHttpsMetadata = false;
                    o.BackchannelHttpHandler = backchannelHttpHandlerFunc.Invoke();
                });

            return services;
        }

        public static IServiceCollection RemoveAuthentication(this IServiceCollection services)
        {
            var servicesToRemove = services.Where(s =>
                s.ServiceType == typeof(IConfigureOptions<AuthenticationOptions>) ||
                s.ServiceType == typeof(JwtBearerHandler) ||
                s.ServiceType == typeof(IPostConfigureOptions<JwtBearerOptions>)
            ).ToList();

            foreach (var service in servicesToRemove)
            {
                services.Remove(service);
            }

            return services;
        }

        public static IServiceCollection AddTestAuthentication(this IServiceCollection services)
        {
            services.AddMvc(opt => opt.Filters.Add(new AllowAnonymousFilter()));
            return services;
        }
    }
}
