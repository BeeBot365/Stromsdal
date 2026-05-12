using Microsoft.AspNetCore.Hosting;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StromsdalKok.Domain.Enums;
using StromsdalKok.Infrastructure.Data;
using StromsdalKok.Infrastructure.Data.Persistence;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("UseInMemory", "true");
    }

    protected override void ConfigureClient(HttpClient client)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
        // Lägg till testdata
        db.Users.Add(new UserEntity
        {
            FirstName = "Alexander",
            LastName = "Test",
            Email = "test@alex.com",
            PasswordHash = BC.HashPassword("Admin123!"),
            Role = UserRole.Admin,
            IsActive = true
        });
        db.SaveChanges();
    }
}