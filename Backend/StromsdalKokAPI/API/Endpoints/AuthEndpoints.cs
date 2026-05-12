using Microsoft.AspNetCore.Mvc;
using StromsdalKok.Application.Interfaces;
using StromsdalKok.Application.DTOs.Auth;

namespace StromsdalKokApi.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder group)
    {
        var authGroup = group.MapGroup("/auth").WithTags("Auth");

        authGroup.MapPost("/login", LoginAsync);
        authGroup.MapPost("/refresh", RefreshAsync);
        authGroup.MapPost("/logout", LogoutAsync);

        return group;
    }

    private static async Task<IResult> LoginAsync([FromBody] LoginRequest request, IAuthService authService, HttpContext context)
    {
        var result = await authService.LoginAsync(request);
        if (result is null) return Results.Unauthorized();

        SetRefreshTokenCookie(context, result.RefreshToken);

        return Results.Ok(new
        {
            result.Token,
            result.FirstName,
            result.LastName,
            result.Role,
            result.ExpiresAt
        });
    }

    private static async Task<IResult> RefreshAsync(IAuthService authService, HttpContext http)
    {
        var refreshToken = http.Request.Cookies["refreshToken"];
        if (refreshToken is null) return Results.Unauthorized();

        var result = await authService.RefreshAsync(refreshToken);
        if (result is null) return Results.Unauthorized();

        SetRefreshTokenCookie(http, result.RefreshToken);

        return Results.Ok(new
        {
            result.Token,
            result.FirstName,
            result.LastName,
            result.Role,
            result.ExpiresAt
        });
    }

    private static async Task<IResult> LogoutAsync(HttpContext http, IAuthService authService)
    {
        var refreshToken = http.Request.Cookies["refreshToken"];
        if (refreshToken is not null)
            await authService.LogoutAsync(refreshToken);

        http.Response.Cookies.Delete("refreshToken");
        return Results.Ok();
    }

    private static void SetRefreshTokenCookie(HttpContext ctx, string refreshToken)
    {
        ctx.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = ctx.Request.IsHttps,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(90)
        });
    }
}