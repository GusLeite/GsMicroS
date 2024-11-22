using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<MongoDBService>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public class MongoDBService
{
    private readonly IMongoCollection<Consumo> _consumptionCollection;

    public MongoDBService(IConfiguration config)
    {
        var mongoSettings = config.GetSection("MongoDB").Get<MongoDBSettings>();
        var client = new MongoClient(mongoSettings.ConnectionString);
        var database = client.GetDatabase(mongoSettings.DatabaseName);
        _consumptionCollection = database.GetCollection<Consumo>(mongoSettings.CollectionName);
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

public class MongoDBSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
}

public class Consumo
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public double Consumption { get; set; }
}
