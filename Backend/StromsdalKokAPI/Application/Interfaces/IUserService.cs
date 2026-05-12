using StromsdalKok.Application.DTOs.User;

namespace StromsdalKok.Application.Interfaces;

public interface IUserService
{
    Task<UserResponse?> GetAsync(int id);
    Task<IEnumerable<UserResponse>> GetAllAsync();
    Task<UserResponse> CreateAsync(CreateUserRequest request);
    Task UpdateAsync(int id, UpdateUserRequest request);
    Task DeactivateAsync(int id);
}