using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NLemos.Api.Framework.Exceptions;
using NLemos.Api.Framework.Tests.Web;
using NLemos.Tests.Shared;
using Xunit;

namespace NLemos.Api.Framework.Tests.Functionals
{
    /// <summary>
    /// The point here is not to throw exception but to handle it gracefully
    /// </summary>
    public class ExceptionTests : HttpTestsBase<Startup>
    {
        public ExceptionTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        protected override void ConfigureTestServices(IServiceCollection services)
        {
        }

        [Fact]
        public async Task Throw_InvalidModelStateException_Returns_422()
        {
            using (var client = CreateClient())
            using (var json = CreateJson(new { Field = "Field", Error = "Field is required" }))
            {
                var result = await client.PostAsync($"/api/Exceptions/{nameof(InvalidModelStateException)}", json);
                result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
                var errorMessage = await result.Content.ReadAsStringAsync();
                errorMessage.Should().Be("[{\"Field\":\"Field\",\"Errors\":[\"Field is required\"]}]");
            }
        }

        [Fact]
        public async Task Throw_InvalidParametersException_Returns_422()
        {
            using (var client = CreateClient())
            using (var json = CreateJson(new { Field = "Field", Error = "Field is required" }))
            {
                var result = await client.PostAsync($"/api/Exceptions/{nameof(InvalidParametersException)}", json);
                result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
                var errorMessage = await result.Content.ReadAsStringAsync();
                errorMessage.Should().Be("[{\"Field\":\"Field\",\"Errors\":[\"Field is required\"]}]");
            }
        }

        [Fact]
        public async Task Throw_KeyNotFoundException_Returns_404()
        {
            using (var client = CreateClient())
            using (var json = CreateJson(new { Field = "Field" }))
            {
                var result = await client.PostAsync($"/api/Exceptions/{nameof(KeyNotFoundException)}", json);
                result.StatusCode.Should().Be(HttpStatusCode.NotFound);
                var errorMessage = await result.Content.ReadAsStringAsync();
                errorMessage.Should().Be("{\"error\": \"Field\"}");
            }
        }
    }
}
