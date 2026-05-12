using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Interfaces.Repositories;
using StromsdalKok.Infrastructure.Data;
using StromsdalKok.Infrastructure.Data.Persistence;
using StromsdalKok.Infrastructure.Mappings;

namespace StromsdalKok.Infrastructure.Repositories;

public class OrderAnalysisRepository : GenericRepository, IOrderAnalysisRepository
{
    public OrderAnalysisRepository(AppDbContext context) : base(context) { }

    public async Task<OrderAnalysis?> GetByOrderIdAsync(int orderId)
    {
        var entity = await GetAsync<OrderAnalysisEntity>(a => a.OrderId == orderId);
        return entity is null ? null : OrderAnalysisMapper.ToDomain(entity);
    }

    public async Task AddAsync(OrderAnalysis orderAnalysis)
    {
        var entity = OrderAnalysisMapper.ToEntity(orderAnalysis);
        await AddEntityAsync(entity);
    }
}