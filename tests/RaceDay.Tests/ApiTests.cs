using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RaceDay.Api.Data;
using Xunit;

namespace RaceDay.Tests;
public class ApiTests : IClassFixture<RaceDayFactory>
{
    private readonly RaceDayFactory _factory;
    public ApiTests(RaceDayFactory factory) => _factory = factory;
    [Fact]
    public async Task ProtectedProfile_RejectsUnauthenticatedRequest()
    {
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/profile");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    [Fact]
    public async Task Register_RejectsShortPassword()
    {
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/register", new { firstName="Lebo", lastName="Mokoena", email=$"short{Guid.NewGuid():N}@example.com", password="123", role="Participant", phone="" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    [Fact]
    public async Task Register_RejectsInvalidRole()
    {
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/register", new { firstName="Lebo", lastName="Mokoena", email=$"role{Guid.NewGuid():N}@example.com", password="StrongPass123!", role="Administrator", phone="" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
public sealed class RaceDayFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<RaceDayDbContext>>();
            services.RemoveAll<RaceDayDbContext>();
            services.AddDbContext<RaceDayDbContext>(options => options.UseInMemoryDatabase("RaceDayTests_" + Guid.NewGuid()));
        });
    }
}
