using StromsdalKok.Domain.Entities;

namespace StromsdalKok.Domain.Interfaces.Repositories;

public interface ITabletDeviceRepository : IRepository<TabletDevice>
{
    Task<TabletDevice?> GetAsync(int id);
    Task<TabletDevice?> GetByDeviceName(string deviceName);
    Task<IEnumerable<TabletDevice>> GetAllAsync();
    Task AddAsync(TabletDevice tabletDevice);
}