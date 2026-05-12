using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StromsdalKok.Application.Interfaces;
using StromsdalKok.Application.Services;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };

                // Tablets skickar sin JWT som httpOnly cookie istället för Authorization-header
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        var hasAuthHeader = ctx.Request.Headers.ContainsKey("Authorization");
                        if (!hasAuthHeader && ctx.Request.Cookies.TryGetValue("device_token", out var deviceToken))
                            ctx.Token = deviceToken;
                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", p => p.RequireRole("Admin"));
            options.AddPolicy("Tablet", p => p.RequireRole("Tablet"));
        });

        return builder;
    }
}