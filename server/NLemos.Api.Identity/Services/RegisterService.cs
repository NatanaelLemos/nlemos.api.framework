using System.Threading.Tasks;
using IdentityServer4.Models;
using NLemos.Api.Framework.Exceptions;
using NLemos.Api.Identity.Data;
using NLemos.Api.Identity.Entities;

namespace NLemos.Api.Identity.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IIdentityRepository _repository;

        public RegisterService(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public Task<User> GetUserByEmail(string email)
        {
            return _repository.GetByEmail(email);
        }

        public async Task<User> RegisterUser(User user)
        {
            var previousUser = await _repository.GetByEmail(user.Email);
            if (previousUser != null)
            {
                throw new InvalidParametersException(nameof(user.Email), "Email is already in use");
            }

            user.Password = user.Password.Sha512();
            await _repository.Add(user);
            return user;
        }

        public async Task<bool> ValidatePassword(string email, string password)
        {
            var user = await GetUserByEmail(email);
            return user.Password == password.Sha512();
        }
    }
}
