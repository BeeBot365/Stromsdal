using BC = BCrypt.Net.BCrypt;
using StromsdalKok.Application.DTOs.User;
using StromsdalKok.Application.Interfaces;
using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Enums;
using StromsdalKok.Domain.ValueObjects;
using StromsdalKok.Domain.Interfaces.Repositories;

namespace StromsdalKok.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse?> GetAsync(int id)
    {
        var user = await _userRepository.GetAsync(id);
        return user is null ? null : ToResponse(user);
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(ToResponse);
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email);
        if (existing is not null)
            throw new InvalidOperationException("En användare med denna e-post finns redan.");

        var role = Enum.Parse<UserRole>(request.Role, ignoreCase: true);
        var name = FullName.Create(request.FirstName, request.LastName);
        var email = Email.Create(request.Email);
        var passwordHash = BC.HashPassword(request.Password);
        PinCode? pinCode;

        if (request.PinCode == null)
            pinCode = null;
        else
            pinCode = PinCode.Create(request.PinCode);

        var user = User.Create(name, email, passwordHash, role);

        await _userRepository.AddAsync(user);

        return ToResponse(user);
    }

    public async Task UpdateAsync(int id, UpdateUserRequest request)
    {
        var user = await _userRepository.GetAsync(id);
        if (user is null)
            throw new InvalidOperationException("Användaren hittades inte.");

        await _userRepository.UpdateNameAndEmailAsync(user);
    }

    public async Task DeactivateAsync(int id)
    {
        var user = await _userRepository.GetAsync(id);
        if (user is null)
            throw new InvalidOperationException("Användaren hittades inte.");

        await _userRepository.DeactivateAsync(id);
    }

    private static UserResponse ToResponse(User user) => new()
    {
        Id = user.Id,
        FirstName = user.Name.FirstName,
        LastName = user.Name.LastName,
        Email = user.Email.Value,
        Role = user.Role.ToString(),
        IsActive = user.IsActive,
    };
}