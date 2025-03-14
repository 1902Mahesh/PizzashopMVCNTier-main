namespace DataLogicLayer.ViewModels;

public class RolePermissionViewModel
{
    public long RoleId { get; set; }
    public string RoleName { get; set; }

    public List<PermissionsViewModel> Permissions { get; set; } = new List<PermissionsViewModel>();
}
