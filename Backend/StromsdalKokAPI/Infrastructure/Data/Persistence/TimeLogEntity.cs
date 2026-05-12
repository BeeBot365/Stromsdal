using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StromsdalKok.Infrastructure.Data.Persistence;

public class TimeLogEntity : BaseEntity
{
    public int OrderId { get; set; }
    public int StationId { get; set; }
    public int UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? TotalMinutes { get; set; }
    public string? Note { get; set; }

    public OrderEntity Order { get; set; } = null!;
    public StationEntity Station { get; set; } = null!;
    public UserEntity User { get; set; } = null!;

    public class Configuration : IEntityTypeConfiguration<TimeLogEntity>
    {
        public void Configure(EntityTypeBuilder<TimeLogEntity> builder)
        {
            builder.ToTable("TimeLogs");

            builder.Property(t => t.StartTime).IsRequired();
            builder.Property(t => t.Note).HasMaxLength(500);

            builder.HasOne(t => t.Order)
                .WithMany(o => o.TimeLogs)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.Station)
                .WithMany(s => s.TimeLogs)
                .HasForeignKey(t => t.StationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.User)
                .WithMany(u => u.TimeLogs)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}