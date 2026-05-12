using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.ValueObjects;
using StromsdalKok.Infrastructure.Data.Persistence;

namespace StromsdalKok.Infrastructure.Mappings;

public static class UserMapper
{
    public static User ToDomain(this UserEntity entity)
    {
        return new User(
            entity.Id,
            FullName.Create(entity.FirstName, entity.LastName),
            Email.Create(entity.Email),
            entity.PasswordHash,
            entity.Role,
            entity.IsActive
        );
    }

    public static UserEntity ToEntity(this User domain)
    {
        return new UserEntity
        {
            Id = domain.Id,
            FirstName = domain.Name.FirstName,
            LastName = domain.Name.LastName,
            Email = domain.Email.Value,
            PasswordHash = domain.PasswordHash,
            Role = domain.Role,
            IsActive = domain.IsActive,
        };
    }
}