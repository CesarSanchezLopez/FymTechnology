using FymTechnology.Domain.Entities;
using FymTechnology.Domain.Interfaces.Repositories;
using FymTechnology.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FymTechnology.Infrastructure.Persistence.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly FymDbContext _context;

    public UserRoleRepository(FymDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<string>> GetRoleNamesByUserIdAsync(Guid userId)
    {
        return await _context.UserRoles
            .Where(x => x.UserId == userId)
            .Select(x => x.Role.Name)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid roleId)
    {
        return await _context.UserRoles
            .AnyAsync(x => x.UserId == userId && x.RoleId == roleId);
    }

    public async Task AddAsync(UserRole userRole)
    {
        await _context.UserRoles.AddAsync(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid userId, Guid roleId)
    {
        var userRole = await _context.UserRoles
            .FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId);

        if (userRole is null)
            return;

        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();
    }
}