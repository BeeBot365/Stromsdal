using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StromsdalKok.Infrastructure.Data.Persistence;

public class TabletDeviceEntity : BaseEntity
{
    public string DeviceName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? Token { get; set; }
    public DateTime? TokenExpiresAt { get; set; }

    // Navigation
    public ICollection<AttendanceEntity> Attendances { get; set; } = new List<AttendanceEntity>();
}

public class TabletDeviceConfiguration : IEntityTypeConfiguration<TabletDeviceEntity>
{
    public void Configure(EntityTypeBuilder<TabletDeviceEntity> builder)
    {
        builder.ToTable("TabletDevices");

        builder.Property(t => t.DeviceName).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Password).IsRequired().HasMaxLength(50);

        builder.HasMany(t => t.Attendances)
            .WithOne(a => a.TabletDevice)
            .HasForeignKey(a => a.TabletDeviceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}