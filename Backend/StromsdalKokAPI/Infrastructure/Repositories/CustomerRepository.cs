using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Interfaces.Repositories;
using StromsdalKok.Infrastructure.Data;
using StromsdalKok.Infrastructure.Data.Persistence;
using StromsdalKok.Infrastructure.Mappings;

namespace StromsdalKok.Infrastructure.Repositories;

public class CustomerRepository : GenericRepository, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context) { }

    public async Task<Customer?> GetAsync(int id)
    {
        var entity = await GetByPkAsync<CustomerEntity>(id);
        return entity is null ? null : CustomerMapper.ToDomain(entity);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var entities = await GetAllAsync<CustomerEntity>();
        return entities.Select(CustomerMapper.ToDomain);
    }

    public async Task AddAsync(Customer customer)
    {
        var entity = CustomerMapper.ToEntity(customer);
        await AddEntityAsync(entity);
    }

    public async Task DeleteAsync(int id) =>
        await DeleteEntityAsync<CustomerEntity>(id);
}