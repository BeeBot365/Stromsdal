using StromsdalKok.Domain.Enums;
using StromsdalKok.Domain.ValueObjects;

namespace StromsdalKok.Domain.Entities;

public class Order : BaseDomain
{
    public int CustomerId { get; private set; }
    public int CreatedByUserId { get; private set; }
    public string OrderNumber { get; private set; }
    public int EstimatedHours { get; private set; }
    public Money Price { get; private set; } = null!;
    public OrderStatus Status { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? DueDate { get; private set; }

    internal Order(int id, int customerId, int createdByUserId,
       string orderNumber, Money price,
       OrderStatus status, DateTime? startDate, DateTime? dueDate, int estimatedHours)
    {
        Id = id;
        CustomerId = customerId;
        CreatedByUserId = createdByUserId;
        OrderNumber = orderNumber;
        Price = price;
        Status = status;
        StartDate = startDate;
        DueDate = dueDate;
        EstimatedHours = estimatedHours;
    }

    private Order(int customerId, int createdByUserId, string orderNumber,
        Money price, DateTime? startDate, DateTime? dueDate, int estimatedHours)
    {
        CustomerId = customerId;
        CreatedByUserId = createdByUserId;
        OrderNumber = orderNumber;
        Price = price;
        Status = OrderStatus.Pending;
        StartDate = startDate;
        DueDate = dueDate;
        EstimatedHours = estimatedHours;
    }

    public static Order Create(
        int customerId,
        int createdByUserId,
        string orderNumber,
        int estimatedHours,
        Money price,
        DateTime? startDate = null,
        DateTime? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new ArgumentException("Ordernummer får inte vara tomt.");

        return new Order(customerId,
        createdByUserId,
        orderNumber.Trim(),
        price, startDate,
        dueDate,
        estimatedHours);
    }

    public void UpdateStatus(OrderStatus status) => Status = status;
    public void UpdatePrice(Money price) => Price = price;
    public void UpdateOrderNumber(string orderNumber) => OrderNumber = orderNumber;
}