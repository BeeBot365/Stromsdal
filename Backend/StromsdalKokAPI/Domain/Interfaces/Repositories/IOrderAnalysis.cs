using StromsdalKok.Domain.Entities;

namespace StromsdalKok.Domain.Interfaces.Repositories;

public interface IOrderAnalysisRepository
{
    Task<OrderAnalysis?> GetByOrderIdAsync(int orderId);
    Task AddAsync(OrderAnalysis orderAnalysis);
}