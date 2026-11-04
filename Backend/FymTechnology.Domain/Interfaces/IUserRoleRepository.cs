using FymTechnology.Domain.Entities;

namespace FymTechnology.Domain.Interfaces.Repositories;

public interface IUserRoleRepository
{
    Task<IEnumerable<string>> GetRoleNamesByUserIdAsync(Guid userId);

    Task<bool> ExistsAsync(Guid userId, Guid roleId);

    Task AddAsync(UserRole userRole);

    Task RemoveAsync(Guid userId, Guid roleId);
}