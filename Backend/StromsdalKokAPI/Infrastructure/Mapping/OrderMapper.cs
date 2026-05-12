using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Enums;
using StromsdalKok.Domain.ValueObjects;
using StromsdalKok.Infrastructure.Data.Persistence;

namespace StromsdalKok.Infrastructure.Mappings;

public static class OrderMapper
{
    public static Order ToDomain(OrderEntity entity)
    {
        return new Order(
            entity.Id,
            entity.CustomerId,
            entity.CreatedByUserId,
            entity.OrderNumber,
            Money.Create(entity.Price, entity.Currency),
            entity.Status,
            entity.StartDate,
            entity.DueDate,
            entity.EstimatedHours
        );
    }

    public static OrderEntity ToEntity(Order domain)
    {
        return new OrderEntity
        {
            Id = domain.Id,
            CustomerId = domain.CustomerId,
            CreatedByUserId = domain.CreatedByUserId,
            OrderNumber = domain.OrderNumber,
            Price = domain.Price.Amount,
            Currency = domain.Price.Currency,
            Status = domain.Status,
            StartDate = domain.StartDate,
            DueDate = domain.DueDate,
        };
    }
}