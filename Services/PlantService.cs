using hcAPI.Interfaces;
using hcAPI.Models;
using hcAPI.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace hcAPI.Services;

public class PlantService : IPlantService
{
    private readonly IMongoCollection<Plant> _plants;

    public PlantService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _plants = db.GetCollection<Plant>("plants");
    }

    public async Task<List<Plant>> GetAllAsync() =>
        await _plants.Find(_ => true).ToListAsync();

    public async Task<Plant?> GetByIdAsync(string id) =>
        await _plants.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Plant plant) =>
        await _plants.InsertOneAsync(plant);

    public async Task UpdateAsync(string id, Plant plant) =>
        await _plants.ReplaceOneAsync(p => p.Id == id, plant);

    public async Task DeleteAsync(string id) =>
        await _plants.DeleteOneAsync(p => p.Id == id);
}