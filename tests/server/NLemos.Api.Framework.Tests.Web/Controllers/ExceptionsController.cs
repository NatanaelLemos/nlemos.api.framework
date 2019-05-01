using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NLemos.Api.Framework.Exceptions;

namespace NLemos.Api.Framework.Tests.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionsController
    {
        [HttpPost("{exceptionType}")]
        public string Post(string exceptionType, [FromBody]ExceptionWrapper exception)
        {
            switch (exceptionType)
            {
                case nameof(KeyNotFoundException):
                    throw new KeyNotFoundException(exception.Field);
                case nameof(InvalidModelStateException):
                    var modelState = new ModelStateDictionary();
                    modelState.AddModelError(exception.Field, exception.Error);
                    throw new InvalidModelStateException(modelState);
                case nameof(InvalidParametersException):
                    throw new InvalidParametersException(exception.Field, exception.Error);
                default:
                    return "";
            }
        }
    }

    public class ExceptionWrapper
    {
        public string Field { get; set; }
        public string Error { get; set; }
    }
}
