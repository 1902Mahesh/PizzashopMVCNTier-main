using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Interfaces;

public interface IRolePermissionService
{
    public RolePermissionViewModel GetRolePermissions(long roleId);

    public Task<bool> EditRolepermissions(long roleId, List<PermissionsViewModel> model, long userId);
}
