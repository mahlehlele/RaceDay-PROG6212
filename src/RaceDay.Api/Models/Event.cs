using System.ComponentModel.DataAnnotations;
namespace RaceDay.Api.Models;
public class Event
{
    public int EventId { get; set; }
    [Required, StringLength(140)] public string Name { get; set; } = "";
    [StringLength(1500)] public string Description { get; set; } = "";
    public DateTime EventDate { get; set; }
    [Required, StringLength(180)] public string Location { get; set; } = "";
    [Range(0.1, 500)] public decimal DistanceKm { get; set; }
    [Required] public string EventType { get; set; } = "Run";
    public int OrganiserId { get; set; }
    public User? Organiser { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    public ICollection<EventRoute> Routes { get; set; } = new List<EventRoute>();
}
