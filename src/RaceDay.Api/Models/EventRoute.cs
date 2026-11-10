using System.ComponentModel.DataAnnotations;
namespace RaceDay.Api.Models;
public class EventRoute
{
    public int EventRouteId { get; set; }
    public int EventId { get; set; }
    public Event? Event { get; set; }
    [Required, StringLength(140)] public string Name { get; set; } = "";
    [StringLength(1000)] public string? Description { get; set; }
    [StringLength(500)] public string? MapUrl { get; set; }
    public decimal? DistanceKm { get; set; }
}
