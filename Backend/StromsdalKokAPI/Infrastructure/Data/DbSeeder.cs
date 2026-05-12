using BC = BCrypt.Net.BCrypt;
using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Enums;
using StromsdalKok.Domain.ValueObjects;
using StromsdalKok.Infrastructure.Mappings;

namespace StromsdalKok.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.Users.Any()) return; // Kör bara om databasen är tom

        var adminUser = UserMapper.ToEntity(
            User.Create(
                FullName.Create("Admin", "Ström"),
                Email.Create("admin@stromsdalkok.se"),
                BC.HashPassword("Admin123!"),
                UserRole.Admin
            )
        );

        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync();

        Console.WriteLine(" Admin-användare skapad!");
        Console.WriteLine("Email: admin@stromsdalkok.se");
        Console.WriteLine("Lösenord: Admin123!");
    }
}