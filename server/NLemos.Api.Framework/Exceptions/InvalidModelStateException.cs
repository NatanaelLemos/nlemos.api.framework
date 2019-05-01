using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NLemos.Api.Framework.Exceptions
{
    public class InvalidModelStateException : InvalidParametersException
    {
        public InvalidModelStateException(ModelStateDictionary modelState) : base(ConvertToInvalidParameters(modelState))
        {
        }

        private static InvalidParameter[] ConvertToInvalidParameters(ModelStateDictionary modelState)
        {
            var invalidParameters = new List<InvalidParameter>();

            foreach (var key in modelState.Keys)
            {
                var modelErrors = modelState[key];
                if (modelErrors == null || !modelErrors.Errors.Any())
                {
                    continue;
                }

                invalidParameters.Add(new InvalidParameter(key, modelErrors.Errors.Select(e => e.ErrorMessage).ToArray()));
            }

            return invalidParameters.ToArray();
        }
    }
}
