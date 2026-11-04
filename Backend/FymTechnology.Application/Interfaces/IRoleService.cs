using FymTechnology.Domain.Entities;

namespace FymTechnology.Application.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<Role>> GetAllAsync();
}