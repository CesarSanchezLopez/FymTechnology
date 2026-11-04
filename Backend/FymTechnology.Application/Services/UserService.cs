using FymTechnology.Application.Common.Exceptions;
using FymTechnology.Application.DTOs.Users;
using FymTechnology.Application.Interfaces;
using FymTechnology.Domain.Entities;
using FymTechnology.Domain.Interfaces.Repositories;

namespace FymTechnology.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> CreateAsync(CreateUserRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var existingUser = await _userRepository.GetByEmailAsync(email);

        if (existingUser is not null)
            throw new ConflictException(
                "El correo electrónico ya está registrado.");

        var role = await _roleRepository.GetByIdAsync(request.RoleId);

        if (role is null)
            throw new NotFoundException(
                "El rol especificado no existe.");

        if (role.Name == "SuperAdmin")
            throw new BusinessRuleException(
                "No es posible asignar el rol SuperAdmin.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        await _userRoleRepository.AddAsync(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            AssignedAt = DateTime.UtcNow
        });

        return user.Id;
    }
    public async Task<IReadOnlyList<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(user => new UserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.IsActive,
            user.CreatedAt,
            user.UserRoles
                .Select(ur => ur.Role.Name)
                .ToList()
        )).ToList();
    }
    public async Task AssignRoleAsync(Guid userId, Guid roleId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
            throw new NotFoundException("El usuario no existe.");

        var role = await _roleRepository.GetByIdAsync(roleId);

        if (role is null)
            throw new NotFoundException("El rol no existe.");

        if (role.Name == "SuperAdmin")
            throw new BusinessRuleException(
                "No es posible asignar el rol SuperAdmin.");

        if (await _userRoleRepository.ExistsAsync(userId, roleId))
            throw new ConflictException(
                "El usuario ya tiene asignado este rol.");

        await _userRoleRepository.AddAsync(new UserRole
        {
            UserId = userId,
            RoleId = roleId,
            AssignedAt = DateTime.UtcNow
        });
    }
}