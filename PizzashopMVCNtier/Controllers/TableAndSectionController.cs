using System.Threading.Tasks;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Implementations;
using BusinessLogicLayer.Interfaces;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PizzashopMVCNtier.Controllers;

public class TableAndSectionController : Controller
{
    private readonly ITableSectionService _tableSectionService;
    private readonly IUserDetailService _userDetailService;


    public TableAndSectionController(ITableSectionService tableSectionService, IUserDetailService userDetailService)
    {
        _tableSectionService = tableSectionService;
        _userDetailService = userDetailService;
    }

/*---------------------------------------------------------------------------Sectoin and Table List------------------------------------------------------------------------------*/

    #region Section and Table List   
    [PermissionAuthorize("TableAndSection_View")]
    public async Task<IActionResult> Index()
    {
        IEnumerable<SectionViewModel> model = await _tableSectionService.GetSectionsList();
        ViewData["ActiveLink"] = "Tables";
        return View(model);
    }

    public async Task<IActionResult> GetTableList(long sectionId = 1, int pageNo = 1, int pageSize = 3, string search = "")
    {
        return PartialView("_tablesList", await _tableSectionService.GetTableList(sectionId, pageNo, pageSize, search));
    }
    #endregion

/*---------------------------------------------------------------------------Sections CRUD------------------------------------------------------------------------------*/

    #region Add/Edit Section
    [HttpGet]
    [PermissionAuthorize("TableAndSection_AddEdit")]
    public async Task<IActionResult> SaveSection(long sectionId)
    {
        SectionViewModel model = new SectionViewModel();

        if (sectionId > 0)
        {
            model = await _tableSectionService.GetSectionById(sectionId);
            return PartialView("_addSection", model);
        }
        return PartialView("_addSection", model);
    }

    [HttpPost]
    [PermissionAuthorize("TableAndSection_AddEdit")]
    public async Task<IActionResult> SaveSection(SectionViewModel model)
    {
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);
        long userId = await _userDetailService.GetUserIdByUserNameAsync(userName);


        if (!ModelState.IsValid)
        {
            TempData["NotificationMessage"] = NotificationMessages.InvalidModelState;
            TempData["NotificationType"] = NotificationType.Error.ToString();
            return RedirectToAction("Index", "TableAndSection");
        }


        string result = "";
        bool isCreated = true;

        //For Adding New Item
        if (model.SectionID == 0)
        {
            result = await _tableSectionService.AddSection(model, userId);
        }
        //For Editing Item
        else
        {
            result = await _tableSectionService.EditSection(model, userId);
            isCreated = false;
        }
        // Checking for Add or Update
        if (result.Equals("true"))
        {
            TempData["NotificationMessage"] = string.Format(isCreated ? NotificationMessages.EntityCreated : NotificationMessages.EntityUpdated, "Section");
            TempData["NotificationType"] = NotificationType.Success.ToString();
        }
        else
        {
            TempData["NotificationMessage"] = result;
            TempData["NotificationType"] = NotificationType.Error.ToString();
        }
        return RedirectToAction("Index", "TableAndSection");
    }
    #endregion

    #region Delete Section
    [HttpPost]
    [PermissionAuthorize("TableAndSection_Delete")]
    public async Task<IActionResult> DeleteSection(long sectionId)
    {
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);

        bool result = await _tableSectionService.DeleteSection(sectionId, userName);
        if (result)
        {
            return Json(new { success = true, message = string.Format(NotificationMessages.EntityDeleted, "Section") });
        }
        else
        {
            return Json(new { success = false, message = string.Format(NotificationMessages.EntityDeletedFailed, "Section") });
        }
    }
    #endregion
}
