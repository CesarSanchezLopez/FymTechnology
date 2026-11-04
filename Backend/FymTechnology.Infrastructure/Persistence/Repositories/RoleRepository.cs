using FymTechnology.Domain.Entities;
using FymTechnology.Domain.Interfaces.Repositories;
using FymTechnology.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FymTechnology.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly FymDbContext _context;

    public RoleRepository(FymDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByIdAsync(Guid id)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _context.Roles
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
    }
}