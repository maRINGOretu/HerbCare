namespace hcAPI.Models;

public enum UserRole { User, Admin }

public class User : BaseDocument
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public List<UserPlant> Garden { get; set; } = new();
    public List<Note> Notes { get; set; } = new();
}