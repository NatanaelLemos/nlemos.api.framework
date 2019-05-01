using NLemos.Api.Framework.Data;

namespace NLemos.Api.Gateway.Data
{
    public class GatewayContext : MongoDbContext
    {
        public GatewayContext(string connectionString, string databaseName) : base(connectionString, databaseName)
        {
        }
    }
}
