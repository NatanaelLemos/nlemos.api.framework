using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NLemos.Api.Framework.Exceptions
{
    /// <summary>
    /// This exception is useful to throw validation exceptions and return them as JSON to the client
    /// Produces a JSON like this:
    /// [
    ///     {
    ///         "Field": "Username",
    ///         "Errors": [
    ///             "Invalid length",
    ///             "Invalid characters"
    ///         ]
    ///     }
    ///]
    /// </summary>
    public class InvalidParametersException : Exception
    {
        public IEnumerable<InvalidParameter> Parameters
        {
            get
            {
                return ((InvalidParametersInnerException)InnerException).Parameters;
            }
        }

        public InvalidParametersException(string field, params string[] errors) : this(new InvalidParameter(field, errors))
        {
        }

        public InvalidParametersException(params InvalidParameter[] parameters) : base("Invalid fields", new InvalidParametersInnerException(parameters))
        {
        }

        //This class exists to split the Exception in two parts:
        //The Exception that is thrown contains "Invalid fields" as message
        //And in the InnerException there is the full message with all fields and errors as Json
        private class InvalidParametersInnerException : Exception
        {
            public IEnumerable<InvalidParameter> Parameters { get; }

            public InvalidParametersInnerException(InvalidParameter[] invalidParameters)
            {
                Parameters = invalidParameters;
            }

            public override string Message
            {
                get
                {
                    var parameters = Parameters.OrderBy(p => p.Field);
                    return JsonConvert.SerializeObject(
                                parameters.Select(p => new
                                {
                                    p.Field,
                                    p.Errors
                                })
                            );
                }
            }
        }
    }
}
