using StromsdalKok.Domain.Entities;

namespace StromsdalKok.Domain.Interfaces.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetByUserIdAsync(int userId);
    Task CreateAsync(RefreshToken token);
    Task UpdateAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
}