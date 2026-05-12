using StromsdalKok.Domain.Entities;

namespace StromsdalKok.Domain.Interfaces.Repositories;

public interface IStationRepository : IRepository<Station>
{
    Task<Station?> GetAsync(int id);
    Task<IEnumerable<Station>> GetAllAsync();
    Task<IEnumerable<Station>> GetAllActiveAsync();
    Task AddAsync(Station station);
    Task DeleteAsync(int id);
}