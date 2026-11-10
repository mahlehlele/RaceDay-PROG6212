using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
namespace RaceDay.Api.Controllers;
[Route("api/categories")]
public class CategoriesController(RaceDayDbContext db) : ApiControllerBase
{
    public record CategoryRequest(int EventId, string Name, string? Description, int? MinimumAge, int? MaximumAge, decimal? DistanceKm);
    [HttpGet] public async Task<IActionResult> GetAll([FromQuery] int? eventId) { var q=db.Categories.AsNoTracking().AsQueryable(); if(eventId.HasValue) q=q.Where(c=>c.EventId==eventId.Value); return Ok(await q.ToListAsync()); }
    [HttpGet("{id:int}")] public async Task<IActionResult> Get(int id) { var c=await db.Categories.AsNoTracking().FirstOrDefaultAsync(x=>x.CategoryId==id); return c is null?NotFound():Ok(c); }
    [HttpPost] public async Task<IActionResult> Create(CategoryRequest request) { if(RequireRole("Organiser") is IActionResult denied)return denied; var e=await db.Events.FindAsync(request.EventId); if(e is null)return NotFound(new{message="Event not found."}); if(e.OrganiserId!=CurrentUserId)return Forbid(); if(string.IsNullOrWhiteSpace(request.Name))return BadRequest(new{message="Category name is required."}); var c=new Category{EventId=request.EventId,Name=request.Name.Trim(),Description=request.Description,MinimumAge=request.MinimumAge,MaximumAge=request.MaximumAge,DistanceKm=request.DistanceKm}; db.Categories.Add(c); await db.SaveChangesAsync(); return CreatedAtAction(nameof(Get),new{id=c.CategoryId},c); }
    [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, CategoryRequest request) { if(RequireRole("Organiser") is IActionResult denied)return denied; var c=await db.Categories.Include(x=>x.Event).FirstOrDefaultAsync(x=>x.CategoryId==id); if(c is null)return NotFound(); if(c.Event!.OrganiserId!=CurrentUserId)return Forbid(); if(c.EventId!=request.EventId || string.IsNullOrWhiteSpace(request.Name))return BadRequest(new{message="A valid category name and matching event ID are required."}); c.Name=request.Name.Trim(); c.Description=request.Description;c.MinimumAge=request.MinimumAge;c.MaximumAge=request.MaximumAge;c.DistanceKm=request.DistanceKm;await db.SaveChangesAsync();return Ok(c); }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) { if(RequireRole("Organiser") is IActionResult denied)return denied; var c=await db.Categories.Include(x=>x.Event).FirstOrDefaultAsync(x=>x.CategoryId==id); if(c is null)return NotFound(); if(c.Event!.OrganiserId!=CurrentUserId)return Forbid(); if(await db.Enrolments.AnyAsync(x=>x.CategoryId==id))return Conflict(new{message="Category is in use by enrolments."});db.Categories.Remove(c);await db.SaveChangesAsync();return NoContent(); }
}
