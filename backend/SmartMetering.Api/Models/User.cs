namespace SmartMetering.Api.Models;

public enum UserRole { Admin, Superviseur, Technicien }

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Technicien;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
