using StromsdalKok.Domain.Enums;
using StromsdalKok.Domain.ValueObjects;

namespace StromsdalKok.Domain.Entities;

public class User : BaseDomain
{
    public FullName Name { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }

    internal User(int id, FullName name, Email email, string passwordHash,
    UserRole role, bool isActive)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = isActive;
    }

    private User(FullName name, Email email, string passwordHash, UserRole role)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = true;
    }

    public static User Create(FullName name, Email email,
    string passwordHash, UserRole role)
    {
        return new User(name, email, passwordHash, role);
    }

    public void SetEmail(string newEmail) => Email = Email.Create(newEmail);
    public void SetName(string firstName, string lastName) => FullName.Create(firstName, lastName);
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}