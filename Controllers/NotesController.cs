using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using hcAPI.Interfaces;
using hcAPI.Models;

namespace hcAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/notes")]
public class NotesController : ControllerBase
{
    private readonly IUserService _users;

    public NotesController(IUserService users) => _users = users;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<IActionResult> GetNotes()
    {
        var user = await _users.GetByIdAsync(UserId);
        return user is null ? NotFound() : Ok(user.Notes);
    }

    [HttpPost]
    public async Task<IActionResult> AddNote(Note note)
    {
        note.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
        note.CreatedAt = DateTime.UtcNow;
        await _users.AddNoteAsync(UserId, note);
        return Ok(note);
    }

    [HttpPut("{noteId}")]
    public async Task<IActionResult> UpdateNote(string noteId, Note note)
    {
        await _users.UpdateNoteAsync(UserId, noteId, note);
        return NoContent();
    }

    [HttpDelete("{noteId}")]
    public async Task<IActionResult> DeleteNote(string noteId)
    {
        await _users.DeleteNoteAsync(UserId, noteId);
        return NoContent();
    }
}