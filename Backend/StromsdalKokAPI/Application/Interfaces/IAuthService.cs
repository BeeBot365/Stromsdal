using StromsdalKok.Application.DTOs.Auth;
using StromsdalKok.Domain.ValueObjects;

namespace StromsdalKok.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task<LoginResponse?> RefreshAsync(string refreshToken);
    Task LogoutAsync(string refreshToken);
    Task<Result> EnrollTablet(EnrollTabletRequest request);
    Task<Result<string>> TabletLoginAsync(TabletLoginRequest request);
    Task<Result<List<FullName>>> GetAllUsers();
}