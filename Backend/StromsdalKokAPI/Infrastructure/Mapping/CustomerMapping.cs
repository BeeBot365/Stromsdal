using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.ValueObjects;
using StromsdalKok.Infrastructure.Data.Persistence;

namespace StromsdalKok.Infrastructure.Mappings;

public static class CustomerMapper
{
    public static Customer ToDomain(CustomerEntity entity)
    {
        return new Customer(
            entity.Id,
            FullName.Create(entity.FirsName, entity.LastName),
            Email.Create(entity.Email),
            entity.Phone,
            entity.Address
        );
    }

    public static CustomerEntity ToEntity(Customer domain)
    {
        return new CustomerEntity
        {
            Id = domain.Id,
            FirsName = domain.Name.FirstName,
            LastName = domain.Name.LastName,
            Email = domain.Email.Value,
            Phone = domain.Phone,
            Address = domain.Address,
        };
    }
}