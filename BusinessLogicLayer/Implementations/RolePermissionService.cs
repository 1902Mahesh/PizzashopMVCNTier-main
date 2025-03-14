using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Implementations;

public class RolePermissionService : IRolePermissionService
{
    private readonly IRolePermissionsRepository _rolePermissionsRepository;


    public RolePermissionService(IRolePermissionsRepository rolePermissionsRepository)
    {
        _rolePermissionsRepository = rolePermissionsRepository;

    }

    public Task<bool> EditRolepermissions(long roleId, List<PermissionsViewModel> model, long userId)
    {
        return _rolePermissionsRepository.EditPermission(roleId, model, userId);
    }


    public RolePermissionViewModel GetRolePermissions(long roleId)
    {
        List<PermissionsViewModel> permissionList = _rolePermissionsRepository.GetRoleAndPermissions(roleId);

        return new RolePermissionViewModel(){
            RoleId = roleId,
            Permissions = permissionList,
        };
    }
}
