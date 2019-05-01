using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLemos.Api.Framework.Extensions.Startup;

namespace NLemos.Api.Framework.Tests.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddBasicServices();
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseErrorHandling()
                .ConfigureBasicApp();
        }
    }
}
