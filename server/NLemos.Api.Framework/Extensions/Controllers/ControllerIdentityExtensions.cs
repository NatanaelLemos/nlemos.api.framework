using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace NLemos.Api.Framework.Extensions.Controllers
{
    public static class ControllerIdentityExtensions
    {
        public static string GetUserEmail(this ControllerBase controller)
        {
            var userEmail = controller?.User?.Claims?.FirstOrDefault(c => c.Type.ToLower().Contains("emailaddress"))?.Value;
            return userEmail;
        }
    }
}
