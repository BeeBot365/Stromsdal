using StromsdalKok.Application.Interfaces;
using StromsdalKok.Application.DTOs.Auth;

namespace StromsdalKokApi.Api.Endpoints;

public static class TabletEndpoints
{
    public static RouteGroupBuilder MapTabletEndpoints(this RouteGroupBuilder group)
    {
        var tabletGroup = group.MapGroup("/auth/tablet").WithTags("Tablet");

        tabletGroup.MapPost("/login", LoginAsync);
        tabletGroup.MapPost("/enroll", EnrollAsync).RequireAuthorization("Admin");
        tabletGroup.MapGet("", GetAllUsersAsync).RequireAuthorization("Tablet");

        return group;
    }

    private static async Task<IResult> LoginAsync(TabletLoginRequest request, IAuthService service, HttpContext http)
    {
        var result = await service.TabletLoginAsync(request);
        if (result.IsFailure) return Results.Unauthorized();

        http.Response.Cookies.Append("device_token", result.Value!, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddYears(1)
        });

        return Results.Ok();
    }

    private static async Task<IResult> EnrollAsync(EnrollTabletRequest request, IAuthService service)
    {
        var result = await service.EnrollTablet(request);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> GetAllUsersAsync(IAuthService service)
    {
        var users = await service.GetAllUsers();
        return Results.Ok(users.Value);
    }
}
