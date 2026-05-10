using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using hcAPI.Interfaces;
using hcAPI.Models;

namespace hcAPI.Controllers;

[ApiController]
[Route("api/plants")]
public class PlantsController : ControllerBase
{
    private readonly IPlantService _plants;
    public PlantsController(IPlantService plants) => _plants = plants;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _plants.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var plant = await _plants.GetByIdAsync(id);
        return plant is null ? NotFound() : Ok(plant);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Plant plant)
    {
        await _plants.CreateAsync(plant);
        return CreatedAtAction(nameof(GetById), new { id = plant.Id }, plant);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Plant plant)
    {
        var existing = await _plants.GetByIdAsync(id);
        if (existing is null) return NotFound();

        plant.Id = id;           // ← перезаписуємо Id з URL
        plant.CreatedAt = existing.CreatedAt; // ← зберігаємо дату створення
        await _plants.UpdateAsync(id, plant);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _plants.DeleteAsync(id);
        return NoContent();
    }
}