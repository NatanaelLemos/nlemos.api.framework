using System.Threading.Tasks;
using NLemos.Api.Identity.Entities;

namespace NLemos.Api.Identity.Services
{
    public interface IRegisterService
    {
        Task<User> GetUserByEmail(string email);

        Task<User> RegisterUser(User user);

        Task<bool> ValidatePassword(string userName, string password);
    }
}
