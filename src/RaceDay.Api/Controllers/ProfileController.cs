using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
namespace RaceDay.Api.Controllers;
[Route("api/profile")]
public class ProfileController(RaceDayDbContext db) : ApiControllerBase
{
    public record UpdateProfileRequest(string FirstName, string LastName, string? Phone, string? ProfileImageUrl);
    [HttpGet]
    public async Task<IActionResult> Get() { if (RequireLogin() is IActionResult denied) return denied; var u = await db.Users.FindAsync(CurrentUserId); return u is null ? NotFound() : Ok(new { u.UserId, u.FirstName, u.LastName, u.Email, u.Role, u.Phone, u.ProfileImageUrl, u.CreatedAt }); }
    [HttpPut]
    public async Task<IActionResult> Update(UpdateProfileRequest request) { if (RequireLogin() is IActionResult denied) return denied; var u = await db.Users.FindAsync(CurrentUserId); if (u is null) return NotFound(); if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName)) return BadRequest(new { message = "First name and last name are required." }); u.FirstName = request.FirstName.Trim(); u.LastName = request.LastName.Trim(); u.Phone = request.Phone?.Trim(); u.ProfileImageUrl = request.ProfileImageUrl?.Trim(); await db.SaveChangesAsync(); return Ok(new { message = "Profile updated.", u.UserId, u.FirstName, u.LastName, u.Email, u.Role, u.Phone, u.ProfileImageUrl }); }
}
