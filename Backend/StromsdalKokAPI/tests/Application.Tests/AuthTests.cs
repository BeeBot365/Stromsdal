using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Enums;
using StromsdalKok.Infrastructure.Data;
using StromsdalKok.Infrastructure.Data.Persistence;
using BC = BCrypt.Net.BCrypt;

public class AuthTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            HandleCookies = true
        });
    }

    [Fact]
    public async Task SuccessfulLoginShouldReturn200OK()
    {
        var body = new { Email = "test@alex.com", Password = "Admin123!" };

        var response = await _client.PostAsJsonAsync("api/auth/login", body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UnsuccessfulLoginShouldReturn401Unauthorized()
    {
        var body = new { Email = "", Password = "" };

        var response = await _client.PostAsJsonAsync("api/auth/login", body);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnRefreshTokenWhenLoggedIn()
    {
        var body = new { Email = "test@alex.com", Password = "Admin123!" };
        var loginResponse = await _client.PostAsJsonAsync("api/auth/login", body);
        loginResponse.EnsureSuccessStatusCode();

        var cookies = loginResponse.Headers.GetValues("Set-Cookie");
        foreach (var cookie in cookies)
            Console.WriteLine($"Cookie: {cookie}");

        var result = await _client.PostAsync("api/auth/refresh", null);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task WhenUserLoggesOutWeShouldDeleteRefrestokenFromCookie()
    {
        var body = new { Email = "test@alex.com", Password = "Admin123!" };
        var loginResponse = await _client.PostAsJsonAsync("api/auth/login", body);
        loginResponse.EnsureSuccessStatusCode();

        var logoutResponse = await _client.PostAsync("api/auth/logout", null);

        var setCookieHeader = logoutResponse.Headers
       .GetValues("Set-Cookie")
       .FirstOrDefault(c => c.Contains("refreshToken"));

        Assert.Equal(HttpStatusCode.OK, logoutResponse.StatusCode);
        Assert.Contains("expires=", setCookieHeader?.ToLower());
    }
}