namespace SmartMetering.Api.Models.Dtos;

public record RegisterRequest(string Email, string Password, string FullName, UserRole Role);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, string Email, string FullName, string Role);
