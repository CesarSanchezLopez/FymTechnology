using FymTechnology.Domain.Entities;

namespace FymTechnology.Domain.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id);

    Task<Role?> GetByNameAsync(string name);

    Task<IEnumerable<Role>> GetAllAsync();

    Task AddAsync(Role role);
}