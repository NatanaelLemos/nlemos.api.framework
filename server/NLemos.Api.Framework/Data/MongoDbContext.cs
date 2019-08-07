using MongoDB.Driver;

namespace NLemos.Api.Framework.Data
{
    public class MongoDbContext
    {
        private IMongoDatabase _database { get; }

        /// <summary>
        /// Creates an instance of <see cref="MongoDbContext"/> defining a connection to a Mongo database.
        /// </summary>
        /// <param name="connectionString">Connection string for the database.</param>
        /// <param name="databaseName">Database name.</param>
        public MongoDbContext(string connectionString, string databaseName)
        {
            var url = new MongoUrl(connectionString);
            var settings = MongoClientSettings.FromUrl(url);
            settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            _database = mongoClient.GetDatabase(databaseName);
        }

        /// <summary>
        /// Gets a enumerable of the entity <typeparamref name="T"/> in a <see cref="DbSet"/> fashion.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="collectionName">The collection name in the database that will be mapped.</param>
        /// <returns>Enumerable of <typeparamref name="T"/></returns>
        protected IMongoCollection<T> GetCollection<T>(string collectionName) => _database.GetCollection<T>(collectionName);
    }
}
