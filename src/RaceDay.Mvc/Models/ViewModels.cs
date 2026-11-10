namespace RaceDay.Mvc.Models;
public record LoginVm(string Email,string Password);
public record RegisterVm(string FirstName,string LastName,string Email,string Password,string Role,string? Phone);
public record EventVm(int EventId,string Name,string Description,DateTime EventDate,string Location,decimal DistanceKm,string EventType,int OrganiserId);
public record CategoryVm(int CategoryId,int EventId,string Name,string? Description,int? MinimumAge,int? MaximumAge,decimal? DistanceKm);
public record ProfileVm(int UserId,string FirstName,string LastName,string Email,string Role,string? Phone,string? ProfileImageUrl);
public record EnrolVm(int EventId,int CategoryId);
public record ResultVm(int ResultId,string EventName,DateTime EventDate,string CategoryName,TimeSpan? FinishTime,int? FinishingPosition,DateTime RecordedAt);
