using System.Threading.Tasks;
using MongoDB.Driver;
using NLemos.Api.Identity.Entities;

namespace NLemos.Api.Identity.Data
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly IdentityContext _context;

        public IdentityRepository(IdentityContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            var result = await _context.Users.FindAsync(filter);
            return await result.FirstOrDefaultAsync();
        }

        public async Task Add(User user)
        {
            await _context.Users.InsertOneAsync(user);
        }
    }
}
