using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Interfaces.Repositories;
using StromsdalKok.Infrastructure.Data;
using StromsdalKok.Infrastructure.Data.Persistence;
using StromsdalKok.Infrastructure.Mappings;

namespace StromsdalKok.Infrastructure.Repositories;

public class UserRepository : GenericRepository, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<User?> GetAsync(int id)
    {
        var entity = await GetByPkAsync<UserEntity>(id);
        return entity is null ? null : UserMapper.ToDomain(entity);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var entity = await GetAsync<UserEntity>(u => u.Email == email.ToLowerInvariant());
        return entity is null ? null : UserMapper.ToDomain(entity);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var entities = await GetAllAsync<UserEntity>();
        return entities.Select(UserMapper.ToDomain);
    }

    public async Task AddAsync(User user)
    {
        var entity = UserMapper.ToEntity(user);
        await AddEntityAsync(entity);
    }

    public async Task DeactivateAsync(int id) =>
        await UpdateEntityAsync<UserEntity>(id, e => e.IsActive = false);

    public async Task DeleteAsync(int id) =>
        await DeleteEntityAsync<UserEntity>(id);

    public async Task UpdateNameAndEmailAsync(User user)
    {
        var userEntity = UserMapper.ToEntity(user);
        await UpdateEntityAsync(userEntity);
    }
}