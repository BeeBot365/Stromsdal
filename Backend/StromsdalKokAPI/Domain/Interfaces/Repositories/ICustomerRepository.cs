using StromsdalKok.Domain.Entities;

namespace StromsdalKok.Domain.Interfaces.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetAsync(int id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    Task DeleteAsync(int id);
}