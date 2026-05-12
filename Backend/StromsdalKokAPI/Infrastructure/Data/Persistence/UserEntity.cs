using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StromsdalKok.Domain.Enums;

namespace StromsdalKok.Infrastructure.Data.Persistence;

public class UserEntity : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }

    // Navigation
    public RefreshTokenEntity? RefreshToken { get; set; }
    public ICollection<OrderEntity> CreatedOrders { get; set; } = new List<OrderEntity>();
    public ICollection<TimeLogEntity> TimeLogs { get; set; } = new List<TimeLogEntity>();
    public ICollection<AttendanceEntity> Attendances { get; set; } = new List<AttendanceEntity>();
}

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(500);
        builder.Property(u => u.Role).IsRequired().HasMaxLength(50);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.CreatedOrders)
            .WithOne(o => o.CreatedByUser)
            .HasForeignKey(o => o.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.TimeLogs)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.Attendances)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}