using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Enums;

namespace StromsdalKok.Domain.Interfaces.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetAsync(int id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId);
    Task AddAsync(Order order);
    Task UpdateStatusAsync(int id, OrderStatus status);
    Task DeleteAsync(int id);
}