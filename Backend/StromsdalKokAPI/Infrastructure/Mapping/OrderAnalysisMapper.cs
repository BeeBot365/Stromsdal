using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.ValueObjects;
using StromsdalKok.Infrastructure.Data.Persistence;

namespace StromsdalKok.Infrastructure.Mappings;

public static class OrderAnalysisMapper
{
    public static OrderAnalysis ToDomain(OrderAnalysisEntity entity)
    {
        return new OrderAnalysis(
            entity.Id,
            entity.OrderId,
            entity.TotalLoggedMinutes,
            Money.Create(entity.TotalLaborCost, entity.TotalLaborCostCurrency),
            Money.Create(entity.OrderPrice, entity.OrderPriceCurrency),
            Money.Create(entity.Margin, entity.MarginCurrency),
            entity.MarginPercent
        );
    }

    public static OrderAnalysisEntity ToEntity(OrderAnalysis domain)
    {
        return new OrderAnalysisEntity
        {
            Id = domain.Id,
            OrderId = domain.OrderId,
            TotalLoggedMinutes = domain.TotalLoggedMinutes,
            TotalLaborCost = domain.TotalLaborCost.Amount,
            TotalLaborCostCurrency = domain.TotalLaborCost.Currency,
            OrderPrice = domain.OrderPrice.Amount,
            OrderPriceCurrency = domain.OrderPrice.Currency,
            Margin = domain.Margin.Amount,
            MarginCurrency = domain.Margin.Currency,
            MarginPercent = domain.MarginPercent,
        };
    }
}