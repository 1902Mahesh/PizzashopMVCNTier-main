using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;

namespace DataLogicLayer.Interfaces;

public interface IRolePermissionsRepository
{
    public List<PermissionsViewModel> GetRoleAndPermissions(long roleId);
    public Task<bool> EditPermission(long roleId, List<PermissionsViewModel> PermissionList, long userId);

    public Task<List<Rolesandpermission>> GetRolesandpermissionsList(long roleId);
}
