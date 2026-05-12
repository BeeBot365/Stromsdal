using StromsdalKok.Domain.Entities;

namespace StromsdalKok.Domain.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task DeleteAsync(int id);
    Task DeactivateAsync(int id);
    Task UpdateNameAndEmailAsync(User user);
}