using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLemos.Api.Framework.Extensions.Startup;
using NLemos.Api.Gateway.Data;
using NLemos.Api.Gateway.Memento;
using NLemos.Api.Gateway.Services;
using NLemos.EventHub;
using NLemos.EventHub.Configuration;

namespace NLemos.Api.Gateway
{
    public class Startup
    {
        private string SwaggerTitle => Configuration.GetSection("Swagger").GetSection("Title").Value;
        private string AuthUrl => Configuration.GetSection("Auth").GetSection("Authority").Value;
        private string AuthAudience => Configuration.GetSection("Auth").GetSection("ScopeName").Value;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton(c => new GatewayContext("whatever", "bla"))
                .AddScoped<IGatewayRepository, GatewayRepository>()
                .AddScoped<IGatewayService, StorageGatewayProxyService>()
                .AddSingleton<IContentTracker, ContentTracker>()
                .AddScoped<GatewayService>();

            services
                .AddBasicServices()
                .AddSwagger(SwaggerTitle)
                .AddAuthentication(AuthUrl, AuthAudience);

            AddEventHub(services);
        }

        private void AddEventHub(IServiceCollection services)
        {
            var hostname = Configuration.GetSection("EventHub").GetSection("Hostname").Value;
            var username = Configuration.GetSection("EventHub").GetSection("Username").Value;
            var password = Configuration.GetSection("EventHub").GetSection("Password").Value;

            var connectionBuilder = new EventHubConnectionBuilder()
                .AddConnection(hostname, username, password, EventHubConnectionType.Server)
                .AddQueue(Config.GatewayQueue);

            services.AddSingleton<IEventHubHandler, EventHubHandler>()
                .AddSingleton(connectionBuilder);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseErrorHandling()
                .UseAuthentication()
                .ConfigureBasicApp()
                .ConfigureSwagger(SwaggerTitle);
        }
    }
}
