using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLemos.Api.Framework.Extensions.Startup;
using NLemos.Api.Identity.Data;
using NLemos.Api.Identity.Dto;
using NLemos.Api.Identity.Tests.Data;
using NLemos.Tests.Shared;
using Xunit;

namespace NLemos.Api.Identity.Tests.Integrated
{
    public class RegisterControllerTests : HttpTestsBase<Startup>
    {
        public RegisterControllerTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        protected override void ConfigureTestServices(IServiceCollection services)
        {
            services
                .AddTestAuthentication("http://localhost", "services_auth", () => CreateHttpMessageHandler())
                .RemoveService<IIdentityRepository>()
                .AddSingleton<IIdentityRepository, FakeIdentityRepository>();
        }

        [Fact]
        public async Task User_Is_Created_And_Retrieven_Successfully()
        {
            var user = new RegisterIn
            {
                Email = "email@email.com",
                Name = "Name",
                Password = "123456"
            };

            var expected = new RegisterOut
            {
                Email = user.Email,
                Name = user.Name
            };

            using (var client = CreateClient())
            using (var json = CreateJson(user))
            {
                var postResult = await client.PostAsync($"/v1/Register", json);
                Assert.Equal(HttpStatusCode.OK, postResult.StatusCode);
                var postJson = await postResult.Content.ReadAsStringAsync();
                JsonConvert.DeserializeObject<RegisterOut>(postJson).Should().BeEquivalentTo(expected);

                await Authenticate(client, user.Email, user.Password);

                var getResult = await client.GetAsync("/v1/Register");
                Assert.Equal(HttpStatusCode.OK, getResult.StatusCode);

                var getJson = await getResult.Content.ReadAsStringAsync();
                JsonConvert.DeserializeObject<RegisterOut>(getJson).Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async Task UnprocessableEntity_If_Two_Users_Are_Registered_With_Same_Email()
        {
            var user = new RegisterIn
            {
                Email = "email@email.com",
                Name = "Name",
                Password = "123456"
            };

            using (var client = CreateClient())
            using (var json = CreateJson(user))
            {
                var postResult = await client.PostAsync($"/v1/Register", json);
                postResult.StatusCode.Should().Be(HttpStatusCode.OK);

                postResult = await client.PostAsync($"/v1/Register", json);
                postResult.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
