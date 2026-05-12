using Microsoft.Extensions.DependencyInjection;
using StromsdalKok.Application.Interfaces;
using StromsdalKok.Application.Services;

namespace StromsdalKok.Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}