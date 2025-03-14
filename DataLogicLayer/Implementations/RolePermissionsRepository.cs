using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DataLogicLayer.Implementations;

public class RolePermissionsRepository : IRolePermissionsRepository
{
    private readonly PizzaShopDbContext _context;

    public RolePermissionsRepository(PizzaShopDbContext context)
    {
        _context = context;

    }

    /*---------------------------------------------------------------------------Get Role and Permissions Method Implementation
    -------------------------------------------------------------------------------------------------------*/
    public List<PermissionsViewModel> GetRoleAndPermissions(long roleId)
    {
        List<Rolesandpermission> permissions =  _context.Rolesandpermissions.Where(rp => rp.Roleid == roleId).Include(rp => rp.Permission).ToList();
        List<PermissionsViewModel> model = new List<PermissionsViewModel>();
        
        foreach(Rolesandpermission perm in permissions){
            model.Add(
                new PermissionsViewModel(){
                    PermissionId = perm.Permissionid,
                    PermissionName = perm.Permission.Name,
                    View = perm.Canview,
                    AddOrEdit = perm.Canaddedit,
                    Delete = perm.Candelete
                }
            );
        }
        return model;
    }
    /*---------------------------------------------------------------------------Edit Permissions Method Implementation
    -------------------------------------------------------------------------------------------------------*/

    public async Task<bool> EditPermission(long roleId, List<PermissionsViewModel> PermissionList, long userId)
    {
       

       if(PermissionList == null){
        return false;
       }

       foreach(PermissionsViewModel permission in PermissionList){
        Rolesandpermission rolePermission = _context.Rolesandpermissions.Where(rp => rp.Roleid == roleId && rp.Permissionid == permission.PermissionId).FirstOrDefault();

        if(rolePermission == null){
            return false;
        }

        rolePermission.Canview = permission.View;
        rolePermission.Canaddedit = permission.AddOrEdit;
        rolePermission.Candelete = permission.Delete;
        rolePermission.UpdatedAt = DateTime.Now;
        rolePermission.UpdatedBy = userId;

        _context.Rolesandpermissions.Update(rolePermission);
        await _context.SaveChangesAsync();
       }
        return true;
    }

    public async Task<List<Rolesandpermission>> GetRolesandpermissionsList(long roleId)
    {
        List<Rolesandpermission> rolesandpermissions = await _context.Rolesandpermissions.Include(rp=>rp.Permission).Where(rp => rp.Roleid == roleId).ToListAsync();
        return rolesandpermissions;
    }

}
