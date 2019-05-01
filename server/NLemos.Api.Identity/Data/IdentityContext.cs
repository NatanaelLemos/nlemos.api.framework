using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NLemos.Api.Framework.Data;
using NLemos.Api.Identity.Entities;

namespace NLemos.Api.Identity.Data
{
    public class IdentityContext : MongoDbContext
    {
        public IdentityContext(string connectionString) : base(connectionString, "servicesIdentity")
        {
            BsonClassMap.RegisterClassMap<User>(cm => { cm.AutoMap(); cm.SetIgnoreExtraElements(true); });
        }

        public IMongoCollection<User> Users => GetCollection<User>("users");
    }
}
