using System.Threading.Tasks;
using BusinessLogicLayer.Constants;
using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PizzashopMVCNtier.Controllers;


public class RolePermissionController :Controller
{
    private readonly IRoleService _roleService;
    private readonly IUserDetailService _userDetailService;
    private readonly IRolePermissionService _rolePermissionService;


    public RolePermissionController(IRoleService roleService, IUserDetailService userDetailService, IRolePermissionService rolePermissionService)
    {
        _roleService = roleService;
        _userDetailService = userDetailService;
        _rolePermissionService = rolePermissionService;

    }

/*---------------------------------------------------------------------------Role List------------------------------------------------------------------------------*/

    #region Role List
    [HttpGet]
    [PermissionAuthorize("RoleAndPermission_View")]
    public async Task<IActionResult> Role(){
        List<Role> roleObj = await _roleService.GetRolesAsync();
        ViewData["Activelink"] = "Role";
        return View(roleObj);
    }

    #endregion

    #region  Permissions Get and Post
    [HttpGet]
    [PermissionAuthorize("RoleAndPermission_AddEdit")]
    public async Task<IActionResult> Permission(long id)
    {
        RolePermissionViewModel model = _rolePermissionService.GetRolePermissions(id);
        Role roleObj = await _roleService.GetRoleByIdAsync(id);
        ViewData["RoleName"] = roleObj.Rolename;
        return View(model);
    }


    [HttpPost]
    [PermissionAuthorize("RoleAndPermission_AddEdit")]
    public async Task<IActionResult> Permission(long roleId, List<PermissionsViewModel> model){
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);

        long userId = await _userDetailService.GetUserIdByUserNameAsync(userName);

        if(model == null){
            ModelState.AddModelError("", "Permission List is Empty");
            return View(model);
        }
        bool result = await _rolePermissionService.EditRolepermissions(roleId ,model, userId);

        if(result){
            return RedirectToAction("Permission", "RolePermission");
        }

        return View(model);
    }
    #endregion
}
