using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
namespace RaceDay.Api.Controllers;
[Route("api/events")]
public class EventsController(RaceDayDbContext db) : ApiControllerBase
{
    public record EventRequest(string Name, string Description, DateTime EventDate, string Location, decimal DistanceKm, string EventType);
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await db.Events.AsNoTracking().Select(e => new { e.EventId, e.Name, e.Description, e.EventDate, e.Location, e.DistanceKm, e.EventType, e.OrganiserId }).ToListAsync());
    [HttpGet("{id:int}")] public async Task<IActionResult> Get(int id) { var e = await db.Events.AsNoTracking().Include(x => x.Categories).Include(x => x.Routes).FirstOrDefaultAsync(x => x.EventId == id); return e is null ? NotFound() : Ok(e); }
    [HttpPost] public async Task<IActionResult> Create(EventRequest request) { if (RequireRole("Organiser") is IActionResult denied) return denied; if (!Valid(request)) return BadRequest(new { message = "Provide valid event details and event type Run, Walk or Cycle." }); var e = new Event { Name = request.Name.Trim(), Description = request.Description?.Trim() ?? "", EventDate = request.EventDate, Location = request.Location.Trim(), DistanceKm = request.DistanceKm, EventType = NormaliseType(request.EventType), OrganiserId = CurrentUserId!.Value }; db.Events.Add(e); await db.SaveChangesAsync(); return CreatedAtAction(nameof(Get), new { id = e.EventId }, e); }
    [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, EventRequest request) { if (RequireRole("Organiser") is IActionResult denied) return denied; var e = await db.Events.FindAsync(id); if (e is null) return NotFound(); if (e.OrganiserId != CurrentUserId) return Forbid(); if (!Valid(request)) return BadRequest(new { message = "Provide valid event details and event type Run, Walk or Cycle." }); e.Name=request.Name.Trim(); e.Description=request.Description?.Trim() ?? ""; e.EventDate=request.EventDate; e.Location=request.Location.Trim(); e.DistanceKm=request.DistanceKm; e.EventType=NormaliseType(request.EventType); await db.SaveChangesAsync(); return Ok(e); }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) { if (RequireRole("Organiser") is IActionResult denied) return denied; var e = await db.Events.FindAsync(id); if (e is null) return NotFound(); if (e.OrganiserId != CurrentUserId) return Forbid(); if (await db.Enrolments.AnyAsync(x => x.EventId == id)) return Conflict(new { message = "An event with enrolments cannot be deleted." }); db.Events.Remove(e); await db.SaveChangesAsync(); return NoContent(); }
    private static bool Valid(EventRequest r) => !string.IsNullOrWhiteSpace(r.Name) && !string.IsNullOrWhiteSpace(r.Location) && r.DistanceKm > 0 && new[]{"Run","Walk","Cycle"}.Contains(NormaliseType(r.EventType));
    private static string NormaliseType(string type) => type?.Trim().ToLowerInvariant() switch { "run" => "Run", "walk" => "Walk", "cycle" => "Cycle", _ => type ?? "" };
}
