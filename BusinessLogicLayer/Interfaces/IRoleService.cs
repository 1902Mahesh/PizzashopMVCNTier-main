using DataLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces;

public interface IRoleService
{
    public Task<List<Role>> GetRolesAsync();

    public Task<Role> GetRoleByIdAsync(long id);
}
