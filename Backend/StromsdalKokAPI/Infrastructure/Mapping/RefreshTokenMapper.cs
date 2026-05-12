using StromsdalKok.Domain.Entities;
using StromsdalKok.Infrastructure.Data.Persistence;

namespace StromsdalKok.Infrastructure.Mappings;

public static class RefreshTokenMapper
{
    public static RefreshToken ToDomain(this RefreshTokenEntity entity) => new()
    {
        Id = entity.Id,
        Token = entity.Token,
        ExpiresAt = entity.ExpiresAt,
        UserId = entity.UserId
    };

    public static RefreshTokenEntity ToEntity(this RefreshToken domain) => new()
    {
        Token = domain.Token,
        ExpiresAt = domain.ExpiresAt,
        UserId = domain.UserId
    };
}