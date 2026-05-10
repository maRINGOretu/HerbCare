using hcAPI.DTOs;
using hcAPI.Interfaces;
using hcAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;

namespace hcAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/garden")]
public class GardenController : ControllerBase
{
    private readonly IUserService _users;
    private readonly IPlantService _plants;

    public GardenController(IUserService users, IPlantService plants)
    {
        _users = users;
        _plants = plants;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<IActionResult> GetGarden()
    {
        var user = await _users.GetByIdAsync(UserId);
        return user is null ? NotFound() : Ok(user.Garden);
    }

    [HttpPost("{plantId}")]
    public async Task<IActionResult> AddPlant(string plantId, [FromQuery] int quantity = 1)
    {
        try
        {
            var plant = await _plants.GetByIdAsync(plantId);

            if (plant is null)
                return NotFound("Рослину не знайдено");

            var userPlant = new UserPlant
            {
                Id = ObjectId.GenerateNewId().ToString(),

                Name = plant.Name,
                Icon = plant.Icon,
                Description = plant.Description,
                MedicinalProperties = plant.MedicinalProperties,
                WateringDays = plant.WateringDays,
                FertilizerType = plant.FertilizerType,
                Category = plant.Category,
                ImageUrl = plant.ImageUrl,
                Quantity = quantity
            };

            await _users.AddToGardenAsync(UserId, userPlant);

            return Ok(userPlant);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpDelete("{userPlantId}")]
    public async Task<IActionResult> RemovePlant(string userPlantId)
    {
        await _users.RemoveFromGardenAsync(UserId, userPlantId);
        return NoContent();
    }
    [HttpPut("{userPlantId}/water")]
    public async Task<IActionResult> WaterPlant(string userPlantId)
    {
        await _users.UpdateUserPlantAsync(UserId, userPlantId, lastWatered: DateTime.UtcNow);
        return NoContent();
    }

    [HttpPut("{userPlantId}/customize")]
    public async Task<IActionResult> CustomizePlant(string userPlantId, [FromBody] CustomizePlantDto dto)
    {
        await _users.UpdateUserPlantAsync(UserId, userPlantId,
            customName: dto.CustomName,
            customDescription: dto.CustomDescription);
        return NoContent();
    }
}