using StromsdalKok.Domain.Entities;

namespace StromsdalKok.Domain.Interfaces.Repositories;

public interface ITimeLogRepository : IRepository<TimeLog>
{
    Task<TimeLog?> GetAsync(int id);
    Task<IEnumerable<TimeLog>> GetByOrderStationIdAsync(int stationId);
    Task<IEnumerable<TimeLog>> GetByUserIdAsync(int userId);
    Task<TimeLog?> GetActiveByUserIdAsync(int userId);
    Task AddAsync(TimeLog timeLog);
    Task StopAsync(int id);
}