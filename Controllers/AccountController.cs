using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using hcAPI.Interfaces;
using hcAPI.Models;

namespace hcAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IUserService _users;

    public AccountController(IUserService users) => _users = users;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpPut("username")]
    public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameDto dto)
    {
        var user = await _users.GetByIdAsync(UserId);
        if (user is null) return NotFound();

        if (await _users.GetByUsernameAsync(dto.Username) != null)
            return BadRequest("Логін вже використовується");

        user.Username = dto.Username;
        await _users.UpdateAsync(UserId, user);
        return Ok();
    }

    [HttpPut("password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
    {
        var user = await _users.GetByIdAsync(UserId);
        if (user is null) return NotFound();

        if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
            return BadRequest("Невірний старий пароль");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _users.UpdateAsync(UserId, user);
        return Ok();
    }
}

public record UpdateUsernameDto(string Username);
public record UpdatePasswordDto(string OldPassword, string NewPassword);