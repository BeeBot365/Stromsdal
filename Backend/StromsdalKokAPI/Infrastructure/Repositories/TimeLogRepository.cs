using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Interfaces.Repositories;
using StromsdalKok.Infrastructure.Data;
using StromsdalKok.Infrastructure.Data.Persistence;
using StromsdalKok.Infrastructure.Mappings;

namespace StromsdalKok.Infrastructure.Repositories;

public class TimeLogRepository : GenericRepository, ITimeLogRepository
{
    public TimeLogRepository(AppDbContext context) : base(context) { }

    public async Task<TimeLog?> GetAsync(int id)
    {
        var entity = await GetByPkAsync<TimeLogEntity>(id);
        return entity is null ? null : TimeLogMapper.ToDomain(entity);
    }

    public async Task<IEnumerable<TimeLog>> GetByUserIdAsync(int userId)
    {
        var entities = await GetManyAsync<TimeLogEntity>(t => t.UserId == userId);
        return entities.Select(TimeLogMapper.ToDomain);
    }

    public async Task<TimeLog?> GetActiveByUserIdAsync(int userId)
    {
        var entity = await GetAsync<TimeLogEntity>(t => t.UserId == userId && t.EndTime == null);
        return entity is null ? null : TimeLogMapper.ToDomain(entity);
    }

    public async Task AddAsync(TimeLog timeLog)
    {
        var entity = TimeLogMapper.ToEntity(timeLog);
        await AddEntityAsync(entity);
    }

    public async Task StopAsync(int id) =>
        await UpdateEntityAsync<TimeLogEntity>(id, e =>
        {
            e.EndTime = DateTime.UtcNow;
            e.TotalMinutes = (int)(DateTime.UtcNow - e.StartTime).TotalMinutes;
        });

    public async Task<IEnumerable<TimeLog>> GetByOrderStationIdAsync(int stationId)
    {
        var entities = await GetManyAsync<TimeLogEntity>(t => t.StationId == stationId);
        return entities.Select(TimeLogMapper.ToDomain);
    }
}