using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Models;
namespace RaceDay.Api.Data;
public class RaceDayDbContext(DbContextOptions<RaceDayDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Enrolment> Enrolments => Set<Enrolment>();
    public DbSet<Result> Results => Set<Result>();
    public DbSet<EventRoute> EventRoutes => Set<EventRoute>();
    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<User>().HasIndex(x => x.Email).IsUnique();
        model.Entity<User>().Property(x => x.Role).HasMaxLength(20);
        model.Entity<Event>().Property(x => x.DistanceKm).HasPrecision(7, 2);
        model.Entity<Category>().Property(x => x.DistanceKm).HasPrecision(7, 2);
        model.Entity<EventRoute>().Property(x => x.DistanceKm).HasPrecision(7, 2);
        model.Entity<Event>().HasOne(x => x.Organiser).WithMany(x => x.OrganisedEvents).HasForeignKey(x => x.OrganiserId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Category>().HasOne(x => x.Event).WithMany(x => x.Categories).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.Cascade);
        model.Entity<Enrolment>().HasOne(x => x.Participant).WithMany(x => x.Enrolments).HasForeignKey(x => x.ParticipantId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Enrolment>().HasOne(x => x.Event).WithMany(x => x.Enrolments).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Enrolment>().HasOne(x => x.Category).WithMany(x => x.Enrolments).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Enrolment>().HasIndex(x => new { x.ParticipantId, x.EventId }).IsUnique();
        model.Entity<Result>().HasOne(x => x.Enrolment).WithOne(x => x.Result).HasForeignKey<Result>(x => x.EnrolmentId).OnDelete(DeleteBehavior.Cascade);
        model.Entity<Result>().HasOne(x => x.RecordedByOrganiser).WithMany().HasForeignKey(x => x.RecordedByOrganiserId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<EventRoute>().HasOne(x => x.Event).WithMany(x => x.Routes).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.Cascade);
    }
}
