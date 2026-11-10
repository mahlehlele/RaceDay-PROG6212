using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
namespace RaceDay.Api.Controllers;
[Route("api/auth")]
public class AuthController(RaceDayDbContext db, IPasswordHasher<User> hasher) : ApiControllerBase
{
    public record RegisterRequest(string FirstName, string LastName, string Email, string Password, string Role, string? Phone);
    public record LoginRequest(string Email, string Password);
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName) || string.IsNullOrWhiteSpace(request.Email) || request.Password.Length < 8)
            return BadRequest(new { message = "First name, last name, email and a password of at least 8 characters are required." });
        var role = request.Role?.Trim();
        if (role is null || !(role.Equals("Organiser", StringComparison.OrdinalIgnoreCase) || role.Equals("Participant", StringComparison.OrdinalIgnoreCase)))
            return BadRequest(new { message = "Role must be Organiser or Participant." });
        var email = request.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(u => u.Email == email)) return Conflict(new { message = "An account with this email already exists." });
        var user = new User { FirstName = request.FirstName.Trim(), LastName = request.LastName.Trim(), Email = email, Role = role.Equals("Organiser", StringComparison.OrdinalIgnoreCase) ? "Organiser" : "Participant", Phone = request.Phone?.Trim() };
        user.PasswordHash = hasher.HashPassword(user, request.Password);
        db.Users.Add(user); await db.SaveChangesAsync();
        return Created("/api/profile", new { user.UserId, user.FirstName, user.LastName, user.Email, user.Role });
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var email = request.Email?.Trim().ToLowerInvariant();
        var user = await db.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user is null || hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password ?? "") == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "Email or password is incorrect." });
        HttpContext.Session.SetInt32("UserId", user.UserId);
        HttpContext.Session.SetString("Role", user.Role);
        HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
        return Ok(new { message = "Login successful.", user.UserId, user.FirstName, user.LastName, user.Email, user.Role, session = "created" });
    }
    [HttpPost("logout")]
    public IActionResult Logout() { HttpContext.Session.Clear(); return Ok(new { message = "You have been logged out." }); }
    [HttpGet("session")]
    public IActionResult SessionStatus() => CurrentUserId is null ? Unauthorized(new { authenticated = false }) : Ok(new { authenticated = true, userId = CurrentUserId, role = CurrentRole });
}
