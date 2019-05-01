using System.Threading.Tasks;
using NLemos.Api.Identity.Entities;

namespace NLemos.Api.Identity.Data
{
    public interface IIdentityRepository
    {
        Task<User> GetByEmail(string email);

        Task Add(User user);
    }
}
