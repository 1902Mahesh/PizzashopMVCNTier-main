using System.Threading.Tasks;
using AspNetCoreGeneratedDocument;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Constants;
using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PizzashopMVCNtier.Controllers;


public class UserController : Controller
{
    private readonly IUserDetailService _userDetailService;
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly ICountryService _countryService;

    public UserController(IUserDetailService userDetailService, IUserService userService, IRoleService roleService, ICountryService countryService)
    {
        _userDetailService = userDetailService;
        _userService = userService;
        _roleService = roleService;
        _countryService = countryService;
    }


    #region User List
    [Authorize]
    public IActionResult Index()
    {
        ViewData["ActiveLink"] = "User";
        return View(new UserViewModel() { UserList = {}, Page = new() });
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserList(int pageNo = 1, int pageSize = 3, string search = "",string columnName="", string sortOrder="")
    {
        return PartialView("_userListPartialView",await _userDetailService.GetUserDetails(pageNo, pageSize, search, columnName, sortOrder));
    }

    #endregion

    #region Add User
    [Authorize(Roles = nameof(UserRoles.SuperAdmin))]
    [HttpGet]
    public async Task<IActionResult> AddUser()
    {
        ViewData["ActiveLink"] = "User";
        return View();
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.SuperAdmin))]
    public async Task<IActionResult> AddUser(AddUserViewModel model)
    {
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);

        if (!ModelState.IsValid)
        {
            TempData["NotificationMessage"] = string.Format(NotificationMessages.EntityCreatedFailed, "User");
            TempData["NotificationType"] = NotificationType.Error.ToString();
            return View(model);
        }
        (string message, bool result) = await _userService.AddUserAsync(model, userName);
        if (result)
        {
            TempData["NotificationMessage"] = string.Format(NotificationMessages.EntityCreated, "User");
            TempData["NotificationType"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", "User");
        }

        // TempData["NotificationMessage"] = string.Format(NotificationMessages.EntityCreatedFailed, "User");
        TempData["NotificationMessage"] = message;
        TempData["NotificationType"] = NotificationType.Error.ToString();
        return RedirectToAction("AddUser", "User");
    }
    #endregion

    #region Edit User
    [HttpGet]
    [Authorize(Roles = nameof(UserRoles.SuperAdmin))]
    public async Task<IActionResult> EditUser(int id)
    {
        EditUserViewModel model = await _userService.GetUserByIdAsync(id);
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.SuperAdmin))]
    public async Task<IActionResult> EditUser(EditUserViewModel model)
    {
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);

        if (!ModelState.IsValid)
        {
            TempData["NotificationMessage"] = string.Format(NotificationMessages.EntityUpdatedFailed, "User"); ;
            TempData["NotificationType"] = NotificationType.Error.ToString();
            await PopulateDropdownLists(model);
            return View(model);
        }
        (string message, bool result) = await _userService.UpdateUserAsync(model, userName);
        if (result)
        {
            TempData["NotificationMessage"] = string.Format(NotificationMessages.EntityUpdated, "User"); ;
            TempData["NotificationType"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", "User");
        }
        TempData["NotificationMessage"] = message;
        TempData["NotificationType"] = NotificationType.Error.ToString();
        await PopulateDropdownLists(model);
        return View(model);
    }
    #endregion

    #region Delete User
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(UserRoles.SuperAdmin))]

    public async Task<IActionResult> DeleteUser(long id)
    {
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);

        bool result = await _userService.DeleteUserAsync(id, userName);
        if (result)
        {
            return Json(new { success = true, message = string.Format(NotificationMessages.EntityDeleted, "User") });
        }
        else
        {
            return Json(new { success = false, message = string.Format(NotificationMessages.EntityDeletedFailed, "User") });
        }
    }
    #endregion

    private async Task PopulateDropdownLists(EditUserViewModel model)
    {
        model.Roles = await _roleService.GetRolesAsync();
        model.Countries = _countryService.GetCountries();
        model.States = model.CountryId > 0 ? _countryService.GetStates(model.CountryId) : new List<State>();
        model.Cities = model.StateId > 0 ?  _countryService.GetCities(model.StateId) : new List<City>();
    }

}
