using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMetering.Api.Data;
using SmartMetering.Api.Models;
using SmartMetering.Api.Models.Dtos;
using SmartMetering.Api.Services;

namespace SmartMetering.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtTokenService _jwt;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthController(AppDbContext db, JwtTokenService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Email == request.Email))
            return Conflict("Un compte existe déjà avec cet email.");

        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            Role = request.Role
        };
        user.PasswordHash = _hasher.HashPassword(user, request.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var token = _jwt.GenerateToken(user);
        return Ok(new AuthResponse(token, user.Email, user.FullName, user.Role.ToString()));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user is null) return Unauthorized("Identifiants invalides.");

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed) return Unauthorized("Identifiants invalides.");

        var token = _jwt.GenerateToken(user);
        return Ok(new AuthResponse(token, user.Email, user.FullName, user.Role.ToString()));
    }
}
