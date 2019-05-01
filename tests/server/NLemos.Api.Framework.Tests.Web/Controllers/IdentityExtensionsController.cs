using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NLemos.Api.Framework.Extensions.Controllers;

namespace NLemos.Api.Framework.Tests.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityExtensionsController : ControllerBase
    {
        [HttpGet("{email}")]
        public string Get(string email)
        {
            if (email != "ignore")
            {
                ControllerContext.HttpContext.User = new ClaimsPrincipal(
                    new List<ClaimsIdentity>
                    {
                        new ClaimsIdentity(new List<Claim>
                        {
                            new Claim("emailaddress", email)
                        })
                    });
            }
            return this.GetUserEmail();
        }
    }
}
