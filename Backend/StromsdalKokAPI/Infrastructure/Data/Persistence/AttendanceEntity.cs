using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StromsdalKok.Domain.Enums;

namespace StromsdalKok.Infrastructure.Data.Persistence;

public class AttendanceEntity : BaseEntity
{
    public int UserId { get; set; }
    public int TabletDeviceId { get; set; }
    public DateTime ClockIn { get; set; }
    public DateTime? ClockOut { get; set; }
    public AttendanceMethod Method { get; set; }
    public int? TotalMinutes { get; set; }

    // Navigation
    public UserEntity User { get; set; } = null!;
    public TabletDeviceEntity TabletDevice { get; set; } = null!;
}

public class AttendanceConfiguration : IEntityTypeConfiguration<AttendanceEntity>
{
    public void Configure(EntityTypeBuilder<AttendanceEntity> builder)
    {
        builder.ToTable("Attendances");

        builder.Property(a => a.Method).IsRequired().HasMaxLength(20);
        builder.Property(a => a.ClockIn).IsRequired();
        builder.Property(a => a.Method).IsRequired().HasConversion<string>();
    }
}