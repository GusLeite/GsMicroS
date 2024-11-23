    using GsMS;
    using Moq;
    using Xunit;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using System.Threading.Tasks;
    using FluentAssertions;

public class MongoDBServiceTests
{
    private readonly MongoDBService _mongoDBService;
    private readonly Mock<IMongoCollection<Consumo>> _mockCollection;

    public MongoDBServiceTests()
    {
        var settings = Options.Create(new MongoDBSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "testdb",
            CollectionName = "testcollection"
        });

        var mockClient = new Mock<IMongoClient>();
        var mockDatabase = new Mock<IMongoDatabase>();
        _mockCollection = new Mock<IMongoCollection<Consumo>>();

        mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(mockDatabase.Object);
        mockDatabase.Setup(d => d.GetCollection<Consumo>(It.IsAny<string>(), null)).Returns(_mockCollection.Object);

        _mongoDBService = new MongoDBService(settings);
    }

    [Fact]
    public async Task CreateAsync_ShouldInsertConsumo()
    {
        var consumo = new Consumo { Id = 1, ValorEmKwh = 100, Nome = "Teste", local = "Local" };

        await _mongoDBService.CreateAsync(consumo);

        _mockCollection.Verify(c => c.InsertOneAsync(consumo, null, default), Times.Once);
    }
}
