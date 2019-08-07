using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using NLemos.Api.Identity.Services;

namespace NLemos.Api.Identity.Security
{
    public class ProfileService : IProfileService
    {
        private readonly IRegisterService _registerService;

        public ProfileService(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var email = context?.Subject?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            var user = await _registerService.GetUserByEmail(email);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Email),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
            };

            claims.AddRange(user.Claims.Select(c => new Claim(JwtClaimTypes.Role, c)));
            context.IssuedClaims = claims;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            //TODO: Check if user is activated
            return Task.FromResult(true);
        }
    }
}
