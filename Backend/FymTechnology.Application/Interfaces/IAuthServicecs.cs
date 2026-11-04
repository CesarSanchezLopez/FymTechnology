using FymTechnology.Application.DTOs.Auth;

namespace FymTechnology.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}