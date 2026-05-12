using StromsdalKok.Domain.Entities;
using StromsdalKok.Infrastructure.Data.Persistence;

namespace StromsdalKok.Infrastructure.Mappings;

public static class TabletDeviceMapper
{
    public static TabletDevice ToDomain(TabletDeviceEntity entity)
    {
        return new TabletDevice(
            entity.Id,
            entity.DeviceName,
            entity.IsActive,
            entity.Password,
            entity.Token,
            entity.TokenExpiresAt
        );
    }

    public static TabletDeviceEntity ToEntity(TabletDevice domain)
    {
        return new TabletDeviceEntity
        {
            Id = domain.Id,
            DeviceName = domain.DeviceName,
            IsActive = domain.IsActive,
            Password = domain.Password,
            Token = domain.Token,
            TokenExpiresAt = domain.TokenExpireAt
        };
    }
}