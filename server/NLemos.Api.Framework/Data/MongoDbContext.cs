using MongoDB.Driver;

namespace NLemos.Api.Framework.Data
{
    public class MongoDbContext
    {
        private IMongoDatabase _database { get; }

        public MongoDbContext(string connectionString, string databaseName)
        {
            var url = new MongoUrl(connectionString);
            var settings = MongoClientSettings.FromUrl(url);
            settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            _database = mongoClient.GetDatabase(databaseName);
        }

        protected IMongoCollection<T> GetCollection<T>(string collectionName) => _database.GetCollection<T>(collectionName);
    }
}
