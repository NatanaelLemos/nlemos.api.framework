using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace NLemos.Tests.Shared
{
    public abstract class HttpTestsBase<TStartup> : IClassFixture<WebApplicationFactory<TStartup>> where TStartup : class
    {
        private readonly WebApplicationFactory<TStartup> _factory;

        public HttpTestsBase(WebApplicationFactory<TStartup> factory)
        {
            _factory = factory.WithWebHostBuilder(b =>
                b.ConfigureTestServices(services =>
                {
                    ConfigureTestServices(services);
                })
                .UseEnvironment("Test"));
        }

        protected abstract void ConfigureTestServices(IServiceCollection services);

        protected HttpMessageHandler CreateHttpMessageHandler()
        {
            return _factory.Server.CreateHandler();
        }

        protected HttpClient CreateClient()
        {
            return _factory.CreateClient();
        }

        protected HttpContent CreateJson(object obj)
        {
            var stringPayload = JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            return httpContent;
        }

        protected HttpContent CreateFormUrlEncoded(Dictionary<string, string> parameters)
        {
            var content = new FormUrlEncodedContent(parameters);
            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            return content;
        }

        protected Dictionary<string, string> GetLoginData(string email, string password)
        {
            return new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_secret", "P@55w0rd"  },
                { "scope", "services_auth" },
                { "client_id", "services_client" },
                { "username", email },
                { "password", password }
            };
        }

        protected async Task Authenticate(HttpClient client, string email, string password)
        {
            var token = await GetAuthToken(client, "email@email.com", "123456");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        protected async Task<string> GetAuthToken(HttpClient client, string email, string password)
        {
            using (var urlEncoded = CreateFormUrlEncoded(GetLoginData(email, password)))
            {
                var response = await client.PostAsync("/connect/token", urlEncoded);
                Assert.Equal(200, (int)response.StatusCode);

                var token = JsonConvert.DeserializeObject<Token>(await response.Content.ReadAsStringAsync());
                return token.access_token;
            }
        }

        protected void Authenticate(HttpClient client)
        {
            /*
                {
                    "Name" : "name",
                    "Email": "email@email.com",
                    "Password": "123456"
                }
             */
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImRiYjhkMTMyYzIxYjZmMDI2NjA5ZWQ1Y2Y3OWU5MGE2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NDg5MTEwNjcsImV4cCI6MTU4MDAxNTA2NywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1NjQ3OCIsImF1ZCI6WyJodHRwOi8vbG9jYWxob3N0OjU2NDc4L3Jlc291cmNlcyIsInNlcnZpY2VzX2F1dGgiXSwiY2xpZW50X2lkIjoic2VydmljZXNfY2xpZW50Iiwic3ViIjoiZW1haWxAZW1haWwuY29tIiwiYXV0aF90aW1lIjoxNTQ4OTExMDY3LCJpZHAiOiJsb2NhbCIsImVtYWlsIjoiZW1haWxAZW1haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsInNjb3BlIjpbInNlcnZpY2VzX2F1dGgiXSwiYW1yIjpbInBhc3N3b3JkIl19.EVmQ42Rs3sw0MXSAWr3PurL5_XMOsfnL4wan6Bn0HbulKQx28r5DGt4FBKGmE72ObyNgLqYpvIDkol230CRn3rNX97eqyv_whC1d-Dv1CUw888_UX9HhnOQMGJ_DgLuGoKU4Sa7NvzcV3eCyl4FHMtgvpnTpBU3AcrYKRbqhnSdTaRV8iTav2tM2mFkohRkZWRGvcDUudMiNduIZpNJgk8LJRMD1qBmmkygrLLqllB8sPvkRjXaoXP9jIFfUiKaTQ_kGfW4ru_f9lsEICEgTkv5Dyuv591PseEknFO3tEIfipawnyhqOuVEs8IfljtzXtV3RNXMkgEW_bagoxiMDpQ");
        }

        private class Token
        {
#pragma warning disable IDE1006 // Naming Styles
            public string access_token { get; set; }
            public string token_type { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        }
    }
}
