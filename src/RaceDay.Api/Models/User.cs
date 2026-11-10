using System.ComponentModel.DataAnnotations;
namespace RaceDay.Api.Models;
public class User
{
    public int UserId { get; set; }
    [Required, StringLength(80)] public string FirstName { get; set; } = "";
    [Required, StringLength(80)] public string LastName { get; set; } = "";
    [Required, EmailAddress, StringLength(180)] public string Email { get; set; } = "";
    [Required] public string PasswordHash { get; set; } = "";
    [Required] public string Role { get; set; } = "Participant";
    [StringLength(30)] public string? Phone { get; set; }
    [StringLength(500)] public string? ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Event> OrganisedEvents { get; set; } = new List<Event>();
    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
}
