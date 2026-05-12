using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Enums;
using StromsdalKok.Domain.Interfaces.Repositories;
using StromsdalKok.Infrastructure.Data;
using StromsdalKok.Infrastructure.Data.Persistence;
using StromsdalKok.Infrastructure.Mappings;

namespace StromsdalKok.Infrastructure.Repositories;

public class OrderRepository : GenericRepository, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context) { }

    public async Task<Order?> GetAsync(int id)
    {
        var entity = await GetByPkAsync<OrderEntity>(id);
        return entity is null ? null : OrderMapper.ToDomain(entity);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        var entities = await GetAllAsync<OrderEntity>();
        return entities.Select(OrderMapper.ToDomain);
    }

    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId)
    {
        var entities = await GetManyAsync<OrderEntity>(o => o.CustomerId == customerId);
        return entities.Select(OrderMapper.ToDomain);
    }

    public async Task AddAsync(Order order)
    {
        var entity = OrderMapper.ToEntity(order);
        await AddEntityAsync(entity);
    }

    public async Task UpdateStatusAsync(int id, OrderStatus status) =>
        await UpdateEntityAsync<OrderEntity>(id, e => e.Status = status);

    public async Task DeleteAsync(int id) =>
        await DeleteEntityAsync<OrderEntity>(id);
}