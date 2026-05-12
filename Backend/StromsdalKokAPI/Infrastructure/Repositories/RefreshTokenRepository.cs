using Microsoft.EntityFrameworkCore;
using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Interfaces.Repositories;
using StromsdalKok.Infrastructure.Data;
using StromsdalKok.Infrastructure.Data.Persistence;
using StromsdalKok.Infrastructure.Mappings;

namespace StromsdalKok.Infrastructure.Repositories;

public class RefreshTokenRepository : GenericRepository, IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext context) : base(context) { }

    public async Task CreateAsync(RefreshToken token)
    {
        var entity = token.ToEntity();
        await AddEntityAsync(entity);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        var entity = await GetAsync<RefreshTokenEntity>(r => r.Token == token);
        return entity?.ToDomain();
    }

    public async Task<RefreshToken?> GetByUserIdAsync(int userId)
    {
        var entity = await GetAsync<RefreshTokenEntity>(r => r.UserId == userId);
        return entity?.ToDomain();
    }

    public async Task UpdateAsync(RefreshToken token)
    {
        var entity = await GetByPkAsync<RefreshTokenEntity>(token.Id);
        if (entity is null) return;

        entity.Token = token.Token;
        entity.ExpiresAt = token.ExpiresAt;
        await UpdateEntityAsync(entity);
    }
}