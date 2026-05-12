using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Interfaces.Repositories;
using StromsdalKok.Infrastructure.Data;
using StromsdalKok.Infrastructure.Data.Persistence;
using StromsdalKok.Infrastructure.Mappings;

namespace StromsdalKok.Infrastructure.Repositories;

public class TabletDeviceRepository : GenericRepository, ITabletDeviceRepository
{
    public TabletDeviceRepository(AppDbContext context) : base(context) { }

    public async Task<TabletDevice?> GetAsync(int id)
    {
        var entity = await GetByPkAsync<TabletDeviceEntity>(id);
        return entity is null ? null : TabletDeviceMapper.ToDomain(entity);
    }

    public async Task<TabletDevice?> GetByDeviceName(string deviceName)
    {
        var entity = await GetAsync<TabletDeviceEntity>(t => t.DeviceName == deviceName);
        return entity is null ? null : TabletDeviceMapper.ToDomain(entity);
    }

    public async Task<IEnumerable<TabletDevice>> GetAllAsync()
    {
        var entities = await GetAllAsync<TabletDeviceEntity>();
        return entities.Select(TabletDeviceMapper.ToDomain);
    }

    public async Task AddAsync(TabletDevice tabletDevice)
    {
        var entity = TabletDeviceMapper.ToEntity(tabletDevice);
        await AddEntityAsync(entity);
    }
}