using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StromsdalKok.Infrastructure.Data.Persistence;

public class OrderAnalysisEntity : BaseEntity
{
    public int OrderId { get; set; }
    public int TotalLoggedMinutes { get; set; }
    public decimal TotalLaborCost { get; set; }
    public string TotalLaborCostCurrency { get; set; } = "SEK";
    public decimal OrderPrice { get; set; }
    public string OrderPriceCurrency { get; set; } = "SEK";
    public decimal Margin { get; set; }
    public string MarginCurrency { get; set; } = "SEK";
    public decimal MarginPercent { get; set; }

    // Navigation
    public OrderEntity Order { get; set; } = null!;
}

public class OrderAnalysisConfiguration : IEntityTypeConfiguration<OrderAnalysisEntity>
{
    public void Configure(EntityTypeBuilder<OrderAnalysisEntity> builder)
    {
        builder.ToTable("OrderAnalyses");

        builder.Property(a => a.TotalLaborCost).HasColumnType("decimal(10,2)");
        builder.Property(a => a.TotalLaborCostCurrency).IsRequired().HasMaxLength(10);
        builder.Property(a => a.OrderPrice).HasColumnType("decimal(10,2)");
        builder.Property(a => a.OrderPriceCurrency).IsRequired().HasMaxLength(10);
        builder.Property(a => a.Margin).HasColumnType("decimal(10,2)");
        builder.Property(a => a.MarginCurrency).IsRequired().HasMaxLength(10);
        builder.Property(a => a.MarginPercent).HasColumnType("decimal(5,2)");

        builder.HasOne(a => a.Order)
            .WithOne(o => o.OrderAnalysis)
            .HasForeignKey<OrderAnalysisEntity>(a => a.OrderId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}