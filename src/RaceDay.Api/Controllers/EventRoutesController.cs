using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
namespace RaceDay.Api.Controllers;
[Route("api/eventroutes")]
public class EventRoutesController(RaceDayDbContext db) : ApiControllerBase
{
    public record RouteRequest(int EventId,string Name,string? Description,string? MapUrl,decimal? DistanceKm);
    [HttpGet] public async Task<IActionResult> Get([FromQuery]int? eventId) { var q=db.EventRoutes.AsNoTracking().AsQueryable();if(eventId.HasValue)q=q.Where(x=>x.EventId==eventId);return Ok(await q.ToListAsync()); }
    [HttpPost] public async Task<IActionResult> Create(RouteRequest request) { if(RequireRole("Organiser") is IActionResult denied)return denied;var e=await db.Events.FindAsync(request.EventId);if(e is null)return NotFound();if(e.OrganiserId!=CurrentUserId)return Forbid();if(string.IsNullOrWhiteSpace(request.Name))return BadRequest(new{message="Route name is required."});var route=new EventRoute{EventId=request.EventId,Name=request.Name.Trim(),Description=request.Description,MapUrl=request.MapUrl,DistanceKm=request.DistanceKm};db.EventRoutes.Add(route);await db.SaveChangesAsync();return Ok(route); }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) { if(RequireRole("Organiser") is IActionResult denied)return denied;var route=await db.EventRoutes.Include(x=>x.Event).FirstOrDefaultAsync(x=>x.EventRouteId==id);if(route is null)return NotFound();if(route.Event!.OrganiserId!=CurrentUserId)return Forbid();db.EventRoutes.Remove(route);await db.SaveChangesAsync();return NoContent(); }
}
