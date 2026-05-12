using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Interfaces.Repositories;
using StromsdalKok.Infrastructure.Data;
using StromsdalKok.Infrastructure.Data.Persistence;
using StromsdalKok.Infrastructure.Mappings;

namespace StromsdalKok.Infrastructure.Repositories;

public class AttendanceRepository : GenericRepository, IAttendanceRepository
{
    public AttendanceRepository(AppDbContext context) : base(context) { }

    public async Task<Attendance?> GetAsync(int id)
    {
        var entity = await GetByPkAsync<AttendanceEntity>(id);
        return entity is null ? null : AttendanceMapper.ToDomain(entity);
    }

    public async Task<IEnumerable<Attendance>> GetByUserIdAsync(int userId)
    {
        var entities = await GetManyAsync<AttendanceEntity>(a => a.UserId == userId);
        return entities.Select(AttendanceMapper.ToDomain);
    }

    public async Task<Attendance?> GetActiveByUserIdAsync(int userId)
    {
        var entity = await GetAsync<AttendanceEntity>(a => a.UserId == userId && a.ClockOut == null);
        return entity is null ? null : AttendanceMapper.ToDomain(entity);
    }

    public async Task AddAsync(Attendance attendance)
    {
        var entity = AttendanceMapper.ToEntity(attendance);
        await AddEntityAsync(entity);
    }

    public async Task ClockOutAsync(int id) =>
        await UpdateEntityAsync<AttendanceEntity>(id, e =>
        {
            e.ClockOut = DateTime.UtcNow;
            e.TotalMinutes = (int)(DateTime.UtcNow - e.ClockIn).TotalMinutes;
        });
}