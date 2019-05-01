using System.Collections.Generic;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace NLemos.Api.Identity
{
    public class Config
    {
        private static IConfigurationRoot _config;

        private static IConfigurationRoot Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
                    _config = builder.Build();
                }
                return _config;
            }
        }

        private static string _scopeName => Configuration.GetSection("Scope").GetSection("ScopeName").Value;
        private static string _scopeDescription => Configuration.GetSection("Scope").GetSection("ScopeDescription").Value;
        private static string _clientId => Configuration.GetSection("Scope").GetSection("ClientId").Value;
        private static string _clientSecret => Configuration.GetSection("Scope").GetSection("ClientSecret").Value;

        public static IEnumerable<Scope> GetScopes()
        {
            return new List<Scope>
            {
                new Scope
                {
                    Name = _scopeName,
                    Description = _scopeDescription
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = _clientId,
                    ClientSecrets =
                    {
                        new Secret(_clientSecret.Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { _scopeName },
                    AccessTokenLifetime = (3600 * 24 * 360),
                    IdentityTokenLifetime = (3600 * 24 * 360)
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),

                new IdentityResource("custom", new[] { "status" })
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource(_scopeName, _scopeDescription)
            };
        }
    }
}
