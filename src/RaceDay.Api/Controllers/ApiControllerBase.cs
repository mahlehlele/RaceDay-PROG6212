using Microsoft.AspNetCore.Mvc;
namespace RaceDay.Api.Controllers;
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected int? CurrentUserId => HttpContext.Session.GetInt32("UserId");
    protected string? CurrentRole => HttpContext.Session.GetString("Role");
    protected IActionResult? RequireLogin() => CurrentUserId is null ? Unauthorized(new { message = "Please log in first." }) : null;
    protected IActionResult? RequireRole(string role)
    {
        if (CurrentUserId is null) return Unauthorized(new { message = "Please log in first." });
        return !string.Equals(CurrentRole, role, StringComparison.OrdinalIgnoreCase) ? Forbid() : null;
    }
}
