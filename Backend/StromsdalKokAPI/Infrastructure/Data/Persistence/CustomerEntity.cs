using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StromsdalKok.Infrastructure.Data.Persistence;

public class CustomerEntity : BaseEntity
{
    public required string FirsName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }

    // Navigation
    public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
}

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.ToTable("Customers");
        builder.Property(c => c.FirsName).IsRequired().HasMaxLength(200);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Email).HasMaxLength(200);
        builder.Property(c => c.Phone).HasMaxLength(50);
        builder.Property(c => c.Address).HasMaxLength(500);

        builder.HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}