using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;

namespace BusinessLogicLayer.Implementations;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;


    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;

    }

    public async Task<Role> GetRoleByIdAsync(long id)
    {
        return await _roleRepository.GetRoleById(id);
    }

    public async Task<List<Role>> GetRolesAsync()
    {
        return await _roleRepository.GetRoles();
    }

}
