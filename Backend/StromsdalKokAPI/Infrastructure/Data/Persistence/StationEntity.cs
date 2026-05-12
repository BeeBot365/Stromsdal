using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StromsdalKok.Infrastructure.Data.Persistence;

public class StationEntity : BaseEntity
{
    public required string Name { get; set; }
    public decimal HourlyCost { get; set; }
    public string Currency { get; set; } = "SEK";
    public bool IsActive { get; set; }

    // Navigation
    public ICollection<TimeLogEntity> TimeLogs { get; set; } = new List<TimeLogEntity>();
}

public class StationConfiguration : IEntityTypeConfiguration<StationEntity>
{
    public void Configure(EntityTypeBuilder<StationEntity> builder)
    {
        builder.ToTable("Stations");

        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.HourlyCost).HasColumnType("decimal(10,2)");
        builder.Property(s => s.Currency).IsRequired().HasMaxLength(10);

        builder.HasMany(s => s.TimeLogs)
            .WithOne(t => t.Station)
            .HasForeignKey(t => t.StationId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}