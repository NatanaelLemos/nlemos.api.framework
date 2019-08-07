using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace NLemos.Api.Framework.Extensions.Controllers
{
    public static class ControllerIdentityExtensions
    {
        /// <summary>
        /// Gets the email address of the logged user from its request header.
        /// </summary>
        /// <param name="controller">The controller that has the request.</param>
        /// <returns>User's email address.</returns>
        public static string GetUserEmail(this ControllerBase controller)
        {
            var userEmail = controller?.User?.Claims?.FirstOrDefault(c => c.Type.ToLower().Contains("emailaddress"))?.Value;
            return userEmail;
        }
    }
}
