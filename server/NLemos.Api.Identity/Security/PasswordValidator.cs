using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using NLemos.Api.Identity.Services;

namespace NLemos.Api.Identity.Security
{
    public class PasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IRegisterService _registerService;

        public PasswordValidator(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var result = await _registerService.ValidatePassword(context.UserName, context.Password);

            if (result)
            {
                context.Result = new GrantValidationResult(context.UserName, "password", null, "local", null);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "The username and password do not match", null);
            }
        }
    }
}
