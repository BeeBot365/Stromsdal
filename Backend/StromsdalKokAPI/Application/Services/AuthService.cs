using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using StromsdalKok.Application.DTOs.Auth;
using StromsdalKok.Application.Interfaces;
using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Interfaces.Repositories;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Logging;
using StromsdalKok.Domain.ValueObjects;

namespace StromsdalKok.Application.Services;

public class AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, ITabletDeviceRepository deviceRepository, IConfiguration configuration, ILogger<AuthService> logger) : IAuthService
{
    // Steg 1. Försöker logga in med email och password
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null) return null;
        if (!user.IsActive) return null;
        if (!BC.Verify(request.Password, user.PasswordHash)) return null;

        return await GenerateResponse(user);
    }

    public async Task<LoginResponse?> RefreshAsync(string refreshToken)
    {
        var existing = await refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (existing is null) return null;
        if (existing.Token is null) return null;
        if (existing.ExpiresAt < DateTime.UtcNow) return null;

        var user = await userRepository.GetAsync(existing.UserId);
        if (user is null) return null;

        var newAccessToken = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        existing.Token = newRefreshToken;
        existing.ExpiresAt = DateTime.UtcNow.AddDays(90);
        await refreshTokenRepository.UpdateAsync(existing);

        return new LoginResponse
        {
            Token = newAccessToken,
            FirstName = user.Name.FirstName,
            LastName = user.Name.LastName,
            Role = user.Role.ToString(),
            ExpiresAt = DateTime.UtcNow.AddMinutes(
                double.Parse(configuration["Jwt:ExpiryMinutes"]!)),
            RefreshToken = newRefreshToken
        };
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var existing = await refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (existing is null) return;

        existing.Token = string.Empty;
        existing.ExpiresAt = DateTime.UtcNow;
        await refreshTokenRepository.UpdateAsync(existing);
    }

    public async Task<Result> EnrollTablet(EnrollTabletRequest request)
    {
        try
        {
            var newTablet = TabletDevice.Create(request.DeviceName, request.Password);
            var existing = await deviceRepository.GetByDeviceName(request.DeviceName);
            if (existing is not null)
                return Result.Failure($"Device med {request.DeviceName} finns redan");

            await deviceRepository.AddAsync(newTablet);

            return Result.Success();
        }
        catch (ArgumentException ex)
        {
            return Result.Failure(ex.Message);
        }
        catch (Exception)
        {
            return Result.Failure("Något gick fel");
        }
    }

    private async Task<LoginResponse> GenerateResponse(User user)
    {
        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        var existing = await refreshTokenRepository.GetByUserIdAsync(user.Id);

        //Om första inloggningen
        if (existing is null)
        {
            await refreshTokenRepository.CreateAsync(new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.Now.AddDays(90),
                UserId = user.Id
            });
        }
        else
        {
            existing.Token = refreshToken;
            existing.ExpiresAt = DateTime.Now.AddDays(90);
            await refreshTokenRepository.UpdateAsync(existing);
        }

        return new LoginResponse
        {
            Token = token,
            FirstName = user.Name.FirstName,
            LastName = user.Name.LastName,
            Role = user.Role.ToString(),
            ExpiresAt = DateTime.UtcNow.AddMinutes(
                double.Parse(configuration["Jwt:ExpiryMinutes"]!)),
            RefreshToken = refreshToken
        };
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email.Value),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.GivenName, user.Name.FirstName),
            new Claim(ClaimTypes.Surname, user.Name.LastName)
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(configuration["Jwt:ExpiryMinutes"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }

    public async Task<Result<string>> TabletLoginAsync(TabletLoginRequest request)
    {
        var result = await deviceRepository.GetByDeviceName(request.DeviceName);
        if (result is null)
        {
            logger.LogWarning($"Device name {request.DeviceName} does not exist.");
            return Result<string>.Failure("");
        }

        if (!BC.Verify(request.Password, result.Password))
        {
            logger.LogWarning($"Wrong password for device name {request.DeviceName}");
            return Result<string>.Failure("");
        }

        if (!result.IsActive)
        {
            logger.LogWarning($"Device {result.DeviceName} is not active");
            return Result<string>.Failure("");
        }

        var token = GenerateTabletToken(result);

        return Result<string>.Success(token);
    }

    private string GenerateTabletToken(TabletDevice tablet)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, tablet.Id.ToString()),
        new Claim("DeviceName", tablet.DeviceName),
        new Claim(ClaimTypes.Role, "Tablet") // så du kan skilja tablet från user i API:et
    };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddYears(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<Result<List<FullName>>> GetAllUsers()
    {
        var result = await userRepository.GetAllAsync();
        var users = result.Where(u => u.IsActive).Select(u => u.Name).ToList();
        return Result<List<FullName>>.Success(users);
    }
}