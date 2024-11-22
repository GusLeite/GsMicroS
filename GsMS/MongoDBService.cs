using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GsMS
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Consumo> _consumoCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var settings = mongoDBSettings.Value;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _consumoCollection = database.GetCollection<Consumo>(settings.CollectionName);
        }

        public async Task CreateAsync(Consumo consumo)
        {
            await _consumoCollection.InsertOneAsync(consumo);
        }

        public async Task<List<Consumo>> GetAsync()
        {
            return await _consumoCollection.Find(_ => true).ToListAsync();
        }
    }
}