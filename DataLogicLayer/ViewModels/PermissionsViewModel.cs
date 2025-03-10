namespace DataLogicLayer.ViewModels;

public class PermissionsViewModel
{
    public long PermissionId { get; set; }
    public string? PermissionName { get; set; }

    public bool View { get; set; }
    public bool AddOrEdit { get; set; }
    public bool Delete { get; set; }

}
