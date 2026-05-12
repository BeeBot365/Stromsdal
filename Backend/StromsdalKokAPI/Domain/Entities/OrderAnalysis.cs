using StromsdalKok.Domain.ValueObjects;

namespace StromsdalKok.Domain.Entities;

public class OrderAnalysis : BaseDomain
{
    public int OrderId { get; private set; }
    public int TotalLoggedMinutes { get; private set; }
    public Money TotalLaborCost { get; private set; } = null!;
    public Money OrderPrice { get; private set; } = null!;
    public Money Margin { get; private set; } = null!;
    public decimal MarginPercent { get; private set; }

    internal OrderAnalysis(int id, int orderId, int totalLoggedMinutes,
        Money totalLaborCost, Money orderPrice, Money margin,
        decimal marginPercent)
    {
        Id = id;
        OrderId = orderId;
        TotalLoggedMinutes = totalLoggedMinutes;
        TotalLaborCost = totalLaborCost;
        OrderPrice = orderPrice;
        Margin = margin;
        MarginPercent = marginPercent;
    }

    private OrderAnalysis(int totalLoggedMinutes,
        Money totalLaborCost, Money orderPrice)
    {
        TotalLoggedMinutes = totalLoggedMinutes;
        TotalLaborCost = totalLaborCost;
        OrderPrice = orderPrice;
        Margin = orderPrice.Subtract(totalLaborCost);
        MarginPercent = orderPrice.Amount > 0
            ? Math.Round(Margin.Amount / orderPrice.Amount * 100, 2)
            : 0;
    }

    public static OrderAnalysis Generate(int totalLoggedMinutes,
        Money totalLaborCost, Money orderPrice)
    {
        return new OrderAnalysis(totalLoggedMinutes,
            totalLaborCost, orderPrice);
    }
}