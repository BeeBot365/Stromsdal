using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StromsdalKok.Domain.Enums;

namespace StromsdalKok.Infrastructure.Data.Persistence;

public class OrderEntity : BaseEntity
{
    public int CustomerId { get; set; }
    public int CreatedByUserId { get; set; }
    public required string OrderNumber { get; set; }
    public decimal Price { get; set; }
    public int EstimatedHours { get; set; }
    public string Currency { get; set; } = "SEK";
    public OrderStatus Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }

    // Navigation
    public CustomerEntity Customer { get; set; } = null!;
    public UserEntity CreatedByUser { get; set; } = null!;
    public ICollection<TimeLogEntity> TimeLogs { get; set; } = new List<TimeLogEntity>();
    public OrderAnalysisEntity? OrderAnalysis { get; set; }
}

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("Orders");

        builder.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);
        builder.Property(o => o.Price).HasColumnType("decimal(10,2)");
        builder.Property(o => o.Currency).IsRequired().HasMaxLength(10);
        builder.Property(o => o.Status).IsRequired().HasConversion<string>().HasMaxLength(50);

        builder.HasIndex(o => o.OrderNumber).IsUnique();

        builder.HasOne(o => o.OrderAnalysis)
            .WithOne(a => a.Order)
            .HasForeignKey<OrderAnalysisEntity>(a => a.OrderId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(o => o.TimeLogs)
            .WithOne(t => t.Order)
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}