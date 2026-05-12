using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Interfaces.Repositories;
using StromsdalKok.Infrastructure.Data;
using StromsdalKok.Infrastructure.Data.Persistence;
using StromsdalKok.Infrastructure.Mappings;

namespace StromsdalKok.Infrastructure.Repositories;

public class StationRepository : GenericRepository, IStationRepository
{
    public StationRepository(AppDbContext context) : base(context) { }

    public async Task<Station?> GetAsync(int id)
    {
        var entity = await GetByPkAsync<StationEntity>(id);
        return entity is null ? null : StationMapper.ToDomain(entity);
    }

    public async Task<IEnumerable<Station>> GetAllAsync()
    {
        var entities = await GetAllAsync<StationEntity>();
        return entities.Select(StationMapper.ToDomain);
    }

    public async Task<IEnumerable<Station>> GetAllActiveAsync()
    {
        var entities = await GetManyAsync<StationEntity>(s => s.IsActive);
        return entities.Select(StationMapper.ToDomain);
    }

    public async Task AddAsync(Station station)
    {
        var entity = StationMapper.ToEntity(station);
        await AddEntityAsync(entity);
    }

    public async Task DeleteAsync(int id) =>
        await DeleteEntityAsync<StationEntity>(id);
}