using hcAPI.Interfaces;
using hcAPI.Models;
using hcAPI.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace hcAPI.Services;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _users = db.GetCollection<User>("users");
    }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

    public async Task<User?> GetByIdAsync(string id) =>
        await _users.Find(u => u.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User user) =>
        await _users.InsertOneAsync(user);

    public async Task UpdateAsync(string id, User user) =>
        await _users.ReplaceOneAsync(u => u.Id == id, user);

    public async Task AddToGardenAsync(string userId, UserPlant userPlant)
    {
        var update = Builders<User>.Update.Push(u => u.Garden, userPlant);
        await _users.UpdateOneAsync(u => u.Id == userId, update);
    }

    public async Task RemoveFromGardenAsync(string userId, string userPlantId)
    {
        var update = Builders<User>.Update
            .PullFilter(u => u.Garden, p => p.UserPlantId == userPlantId);
        await _users.UpdateOneAsync(u => u.Id == userId, update);
    }
    public async Task<User?> GetByUsernameAsync(string username) =>
    await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
    public async Task AddNoteAsync(string userId, Note note)
    {
        var update = Builders<User>.Update.Push(u => u.Notes, note);
        await _users.UpdateOneAsync(u => u.Id == userId, update);
    }
    public async Task UpdateNoteAsync(string userId, string noteId, Note note)
    {
        var user = await GetByIdAsync(userId);
        if (user == null) return;

        var existing = user.Notes.FirstOrDefault(n => n.Id == noteId);
        if (existing == null) return;

        existing.Title = note.Title;
        existing.Content = note.Content;
        existing.NoteColor = note.NoteColor;
        existing.LinkedPlantName = note.LinkedPlantName;
        existing.UserPlantId = note.UserPlantId;
        existing.Icon = note.Icon;
        existing.UpdatedAt = DateTime.UtcNow;

        await UpdateAsync(userId, user);
    }
    public async Task DeleteNoteAsync(string userId, string noteId)
    {
        var update = Builders<User>.Update
            .PullFilter(u => u.Notes, n => n.Id == noteId);
        await _users.UpdateOneAsync(u => u.Id == userId, update);
    }
    public async Task<List<User>> GetAllAsync() =>
    await _users.Find(_ => true).ToListAsync();

    public async Task DeleteAsync(string id) =>
        await _users.DeleteOneAsync(u => u.Id == id);
    public async Task UpdateUserPlantAsync(string userId, string userPlantId,
    DateTime? lastWatered = null,
    string? customName = null,
    string? customDescription = null)
    {
        var user = await GetByIdAsync(userId);
        if (user == null) return;

        var plant = user.Garden.FirstOrDefault(p => p.UserPlantId == userPlantId);
        if (plant == null) return;

        if (lastWatered.HasValue) plant.LastWatered = lastWatered;
        if (customName != null) plant.CustomName = customName;
        if (customDescription != null) plant.CustomDescription = customDescription;

        await UpdateAsync(userId, user);
    }
}