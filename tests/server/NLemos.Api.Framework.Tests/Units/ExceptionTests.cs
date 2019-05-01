using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NLemos.Api.Framework.Exceptions;
using Xunit;

namespace NLemos.Api.Framework.Tests.Units
{
    public class ExceptionTests
    {
        [Fact]
        public void InvalidParametersException_Serializes_Correctly()
        {
            var ex = new InvalidParametersException("Field", "This field is required");
            var expected = "[{\"Field\":\"Field\",\"Errors\":[\"This field is required\"]}]";

            ex.Message.Should().Be("Invalid fields");
            ex.InnerException.Message.Should().Be(expected);
        }

        [Fact]
        public void InvalidParametersException_Creates_Parameters_Correctly()
        {
            var ex = new InvalidParametersException("Field", "This field is required");

            ex.Parameters.Should().HaveCount(1);
            ex.Parameters.First().Field.Should().Be("Field");

            ex.Parameters.First().Errors.Should().HaveCount(1);
            ex.Parameters.First().Errors.First().Should().Be("This field is required");
        }

        [Fact]
        public void InvalidParametersException_Serializes_InvalidParameters_Properly()
        {
            var ex = new InvalidParametersException(
                new InvalidParameter("Field", "This field is required")
            );
            var expected = "[{\"Field\":\"Field\",\"Errors\":[\"This field is required\"]}]";
            ex.InnerException.Message.Should().Be(expected);
        }

        [Fact]
        public void InvalidModelStateException_Serializes_Correctly()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("Field", "This field is required");

            var ex = new InvalidModelStateException(modelState);
            var expected = "[{\"Field\":\"Field\",\"Errors\":[\"This field is required\"]}]";

            ex.InnerException.Message.Should().Be(expected);
        }
    }
}
