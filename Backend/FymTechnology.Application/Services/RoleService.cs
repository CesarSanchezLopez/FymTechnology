using FymTechnology.Application.Interfaces;
using FymTechnology.Domain.Entities;
using FymTechnology.Domain.Interfaces.Repositories;

namespace FymTechnology.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _roleRepository.GetAllAsync();
    }
}