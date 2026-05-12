using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StromsdalKok.Domain.Interfaces.Repositories;
using StromsdalKok.Infrastructure.Data;
using StromsdalKok.Infrastructure.Repositories;

namespace StromsdalKok.Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (configuration["UseInMemory"] == "true")
            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("TestDb"));
        else
            services.AddDbContext<AppDbContext>(o =>
                o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IStationRepository, StationRepository>();
        services.AddScoped<ITimeLogRepository, TimeLogRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<ITabletDeviceRepository, TabletDeviceRepository>();
        services.AddScoped<IOrderAnalysisRepository, OrderAnalysisRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }
}