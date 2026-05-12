using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.ValueObjects;
using StromsdalKok.Infrastructure.Data.Persistence;

namespace StromsdalKok.Infrastructure.Mappings;

public static class StationMapper
{
    public static Station ToDomain(StationEntity entity)
    {
        return new Station(
            entity.Id,
            entity.Name,
            Money.Create(entity.HourlyCost, entity.Currency),
            entity.IsActive
        );
    }

    public static StationEntity ToEntity(Station domain)
    {
        return new StationEntity
        {
            Id = domain.Id,
            Name = domain.Name,
            HourlyCost = domain.HourlyCost.Amount,
            Currency = domain.HourlyCost.Currency,
            IsActive = domain.IsActive
        };
    }
}