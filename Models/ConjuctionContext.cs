using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ConjuctorAPI.Models
{
    public class ConjuctionContext
    {
        private readonly IMongoDatabase _database = null;

        public ConjuctionContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Conjuction> Conjuctions
        {
            get
            {
                return _database.GetCollection<Conjuction>("Conjuctions");
            }
        }
    }
}