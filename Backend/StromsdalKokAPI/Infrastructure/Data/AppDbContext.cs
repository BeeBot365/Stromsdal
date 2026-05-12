using StromsdalKok.Infrastructure.Data.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace StromsdalKok.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<StationEntity> Stations => Set<StationEntity>();
    public DbSet<TimeLogEntity> TimeLogs => Set<TimeLogEntity>();
    public DbSet<AttendanceEntity> Attendances => Set<AttendanceEntity>();
    public DbSet<TabletDeviceEntity> TabletDevices => Set<TabletDeviceEntity>();
    public DbSet<OrderAnalysisEntity> OrderAnalyses => Set<OrderAnalysisEntity>();
    public DbSet<RefreshTokenEntity> RefresTokens => Set<RefreshTokenEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<BaseEntity>();
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        // base.OnModelCreating(modelBuilder);
    }
}