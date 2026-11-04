using FymTechnology.Application.DTOs.Auth;
using FymTechnology.Application.Interfaces;
using FymTechnology.Domain.Interfaces.Repositories;

namespace FymTechnology.Infrastructure.Security;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IJwtTokenService jwtTokenService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null || !user.IsActive)
            return null;

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            return null;

        var roles = await _userRoleRepository.GetRoleNamesByUserIdAsync(user.Id);

        var token = await _jwtTokenService.GenerateTokenAsync(
            user.Id,
            user.Email,
            roles.ToList());

        var expiresAt = DateTime.UtcNow.AddMinutes(60);

        return new LoginResponse(token, expiresAt);
    }
}