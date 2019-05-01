using System.Net;
using System.Threading.Tasks;
using System.Web;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NLemos.Api.Framework.Tests.Web;
using NLemos.Tests.Shared;
using Xunit;

namespace NLemos.Api.Framework.Tests.Functionals
{
    public class ControllerIdentityExtensionsTests : HttpTestsBase<Startup>
    {
        public ControllerIdentityExtensionsTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        protected override void ConfigureTestServices(IServiceCollection services)
        {
        }

        [Fact]
        public async Task GetUserEmail_Returns_User_Email_For_Valid_Claims()
        {
            using (var client = CreateClient())
            {
                var expected = "email@email.com";
                var encodedEmail = HttpUtility.UrlEncode(expected);
                var result = await client.GetAsync($"/api/IdentityExtensions/{encodedEmail}");

                result.StatusCode.Should().Be(HttpStatusCode.OK);

                var decodedEmail = await result.Content.ReadAsStringAsync();
                decodedEmail.Should().Be(expected);
            }
        }

        [Fact]
        public async Task GetUserEmail_Does_Not_Throw_If_Email_Is_Null()
        {
            using (var client = CreateClient())
            {
                var result = await client.GetAsync($"/api/IdentityExtensions/ignore");
                result.StatusCode.Should().Be(HttpStatusCode.NoContent);

                var decodedEmail = await result.Content.ReadAsStringAsync();
                decodedEmail.Should().Be("");
            }
        }
    }
}
