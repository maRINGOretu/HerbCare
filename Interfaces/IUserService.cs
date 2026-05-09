using hcAPI.Models;

namespace hcAPI.Interfaces;

public interface IUserService
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByUsernameAsync(string username);
    Task CreateAsync(User user);
    Task UpdateAsync(string id, User user);
    Task AddToGardenAsync(string userId, UserPlant userPlant);
    Task RemoveFromGardenAsync(string userId, string userPlantId);
    Task AddNoteAsync(string userId, Note note);
    Task UpdateNoteAsync(string userId, string noteId, Note note);
    Task DeleteNoteAsync(string userId, string noteId);
    Task<List<User>> GetAllAsync();
    Task DeleteAsync(string id);
    Task UpdateUserPlantAsync(string userId, string userPlantId,
    DateTime? lastWatered = null,
    string? customName = null,
    string? customDescription = null);
}