using Microsoft.AspNetCore.Builder;
using NLemos.Api.Framework.Middlewares;

namespace NLemos.Api.Framework.Extensions.Startup
{
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandling>();
            return app;
        }
    }
}
