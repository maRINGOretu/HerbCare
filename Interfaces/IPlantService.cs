using hcAPI.Models;

namespace hcAPI.Interfaces;

public interface IPlantService
{
    Task<List<Plant>> GetAllAsync();
    Task<Plant?> GetByIdAsync(string id);
    Task CreateAsync(Plant plant);
    Task UpdateAsync(string id, Plant plant);
    Task DeleteAsync(string id);
}