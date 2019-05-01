using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace NLemos.Api.Framework.Extensions.Startup
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, string title)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = title, Version = "v1" });
            });

            return services;
        }

        public static void ConfigureSwagger(this IApplicationBuilder app, string title)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{title}");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
