using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StromsdalKok.Infrastructure.Data.Persistence;

public class RefreshTokenEntity : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    //Navigation

}

public class RefresTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.Property(r => r.Token).HasMaxLength(500);

        builder.HasOne(r => r.User)
             .WithOne(u => u.RefreshToken)
             .HasForeignKey<RefreshTokenEntity>(r => r.UserId)
             .OnDelete(DeleteBehavior.Cascade);
    }
}