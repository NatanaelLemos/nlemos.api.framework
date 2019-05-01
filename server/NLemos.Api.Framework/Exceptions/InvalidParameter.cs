using System.Collections.Generic;

namespace NLemos.Api.Framework.Exceptions
{
    public class InvalidParameter
    {
        public string Field { get; }

        public IEnumerable<string> Errors { get; }

        public InvalidParameter(string field, params string[] errors)
        {
            Field = field;
            Errors = errors;
        }
    }
}