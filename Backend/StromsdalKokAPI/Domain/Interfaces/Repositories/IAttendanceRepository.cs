using StromsdalKok.Domain.Entities;

namespace StromsdalKok.Domain.Interfaces.Repositories;

public interface IAttendanceRepository : IRepository<Attendance>
{
    Task<Attendance?> GetAsync(int id);
    Task<IEnumerable<Attendance>> GetByUserIdAsync(int userId);
    Task<Attendance?> GetActiveByUserIdAsync(int userId);
    Task AddAsync(Attendance attendance);
    Task ClockOutAsync(int id);
}