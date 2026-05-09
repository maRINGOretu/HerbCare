namespace hcAPI.DTOs;

public record RegisterDto(string Username, string Email, string Password);
public record LoginDto(string Email, string Password);
public record AuthResponseDto(string Token, string UserId, string Username, string Email, string Role, DateTime CreatedAt);