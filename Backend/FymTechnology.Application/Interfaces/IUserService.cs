using FymTechnology.Application.DTOs.Users;

namespace FymTechnology.Application.Interfaces;

public interface IUserService
{
    Task<Guid> CreateAsync(CreateUserRequest request);
    Task<IReadOnlyList<UserResponse>> GetAllAsync();
    Task AssignRoleAsync(Guid userId, Guid roleId);
}