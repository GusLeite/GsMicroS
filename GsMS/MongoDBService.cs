using MongoDB.Driver;

namespace GsMS
{
    public class MongoDBService
    {
        private const string DB_URL = "mongodb://localhost:27017";
        private readonly IMongoCollection<Consumo> _consumptionCollection;

        public MongoDBService(IConfiguration config)
        {
            var client = new MongoClient(DB_URL);
            var database = client.GetDatabase("EnergyConsumptionDB");
            _consumptionCollection = database.GetCollection<Consumo>("Consumos");
        }

        public async Task CreateAsync(Consumo consumption)
        {
            await _consumptionCollection.InsertOneAsync(consumption);
        }

        public async Task<List<Consumo>> GetAsync()
        {
            return await _consumptionCollection.Find(_ => true).ToListAsync();
        }
    }
}