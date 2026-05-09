using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using hcAPI.Interfaces;

namespace hcAPI.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IUserService _users;
    private readonly IPlantService _plants;

    public AdminController(IUserService users, IPlantService plants)
    {
        _users = users;
        _plants = plants;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _users.GetAllAsync();
        var result = users.Select(u => new
        {
            u.Id,
            u.Username,
            u.Email,
            Role = u.Role.ToString(),
            u.CreatedAt,
            GardenCount = u.Garden.Count
        });
        return Ok(result);
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var users = await _users.GetAllAsync();
        var plants = await _plants.GetAllAsync();

        var mostPopular = users
            .SelectMany(u => u.Garden)
            .GroupBy(p => p.Name)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key ?? "Немає даних";

        var totalNotes = users.Sum(u => u.Notes.Count);

        return Ok(new
        {
            TotalUsers = users.Count,
            TotalPlants = plants.Count,
            MostPopularPlant = mostPopular,
            TotalNotes = totalNotes
        });
    }

    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        await _users.DeleteAsync(userId);
        return NoContent();
    }
}