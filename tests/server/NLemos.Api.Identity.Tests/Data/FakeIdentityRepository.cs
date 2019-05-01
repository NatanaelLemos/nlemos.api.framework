using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLemos.Api.Identity.Data;
using NLemos.Api.Identity.Entities;

namespace NLemos.Api.Identity.Tests.Data
{
    public class FakeIdentityRepository : IIdentityRepository
    {
        public List<User> Data { get; set; } = new List<User>();

        public Task Add(User user)
        {
            Data.Add(user);
            return Task.CompletedTask;
        }

        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(Data.FirstOrDefault(d => d.Email == email));
        }
    }
}