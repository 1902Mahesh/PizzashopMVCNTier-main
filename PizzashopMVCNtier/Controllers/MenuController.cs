using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Implementations;
using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PizzashopMVCNtier.Controllers;

[Authorize]
public class MenuController : Controller
{
    private readonly ICategoryItemService _categoryItemService;
    private readonly IUserDetailService _userDetailService;
    private readonly IModifiersService _modifiersService;

    public MenuController(ICategoryItemService categoryItemService, IUserDetailService userDetailService, IModifiersService modifiersService)
    {
        _categoryItemService = categoryItemService;
        _userDetailService = userDetailService;
        _modifiersService = modifiersService;

    }

    /*---------------------------------------------------------------------------Menu Page Index------------------------------------------------------------------------------*/

    #region Menu Home Page
    [HttpGet]
    [PermissionAuthorize("Menu_View")]
    public IActionResult Index()
    {
        MenuViewModel model = new()
        {
            CategoryList = _categoryItemService.GetCategories(),
            ItemModel = new ItemListViewModel() { ItemList = { }, Page = new() },
            AddItems = new AdditemViewModel()
        };
        ViewData["ActiveLink"] = "Menu";
        return View(model);
    }
    #endregion

    /*---------------------------------------------------------------------------Category CRUD---------------------------------------------------------------------------------*/

    #region Get Categories
    public IActionResult GetCategoryById(long id)
    {
        return Json(_categoryItemService.GetCategoryById(id));
    }
    #endregion

    #region Add/Edit Categories
    [HttpPost]
    [PermissionAuthorize("Menu_AddEdit")]
    public async Task<IActionResult> SaveCategory(MenuViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index", "Menu");
        }
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);

        bool result = false;
        bool isCreated = true;

        //For Adding New Category
        if (model.Category.Id == -1)
        {
            result = await _categoryItemService.AddCategory(model.Category, userName);
        }
        //For Editing Category
        else
        {
            result = await _categoryItemService.EditCategory(model.Category, userName);
            isCreated = false;
        }
        // Checking for Add or Update
        if (result)
        {
            TempData["NotificationMessage"] = string.Format(isCreated ? NotificationMessages.EntityCreated : NotificationMessages.EntityUpdated, "Category");
            TempData["NotificationType"] = NotificationType.Success.ToString();
        }
        else
        {
            TempData["NotificationMessage"] = string.Format(isCreated ? NotificationMessages.EntityCreatedFailed : NotificationMessages.EntityUpdatedFailed, "Category");
            TempData["NotificationType"] = NotificationType.Error.ToString();
        }
        return RedirectToAction("Index", "Menu");
    }

    #endregion

    #region Delete Category
    [HttpPost]
    [PermissionAuthorize("Menu_Delete")]
    public async Task<IActionResult> DeleteCategory(long id)
    {
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);

        bool result = await _categoryItemService.DeleteCategory(id, userName);
        if (result)
        {
            return Json(new { success = true, message = string.Format(NotificationMessages.EntityDeleted, "Category") });
        }
        else
        {
            return Json(new { success = false, message = string.Format(NotificationMessages.EntityDeletedFailed, "Category") });
        }
    }
    #endregion

    /*---------------------------------------------------------------------------Items CRUD-----------------------------------------------------------------------------------*/

    #region Get Items List
    public async Task<IActionResult> GetItemList(long categoryId = 1, int pageNo = 1, int pageSize = 3, string search = "")
    {
        return PartialView("_itemsPartialView", await _categoryItemService.GetItemList(categoryId, pageNo, pageSize, search));
    }
    #endregion

    #region Add/Edit Items
    [HttpGet]
    // [PermissionAuthorize("Menu_AddEdit")]
    public async Task<IActionResult> SaveItem(long id)
    {
        //for the Add Item Model
        AdditemViewModel model = new AdditemViewModel();
        model.ItemModifierList = new List<ItemModifierGroupListViewModel>();

        //Fetch the Item details for Edit Item Modal
        if (id > 0)
        {
            model = await _categoryItemService.GetItemByID(id);
            model.ItemModifierList = await _modifiersService.GetAllModifierItemById(id);
        }
        model.CategoryList = _categoryItemService.GetCategories();
        model.ItemTypeList = _categoryItemService.GetItemtypes();
        model.UnitList = _categoryItemService.GetUnits();
        model.ModifierGropList = _modifiersService.GetAllModifierGroup();

        return PartialView("_addItemModalPartialView", model);
    }

    [HttpPost]
    [PermissionAuthorize("Menu_AddEdit")]
    public async Task<IActionResult> SaveItem(AdditemViewModel model, string modifierItemList)
    {
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);
        long userId = await _userDetailService.GetUserIdByUserNameAsync(userName);
        if (!string.IsNullOrEmpty(modifierItemList))
        {
            model.ItemModifierList = JsonSerializer.Deserialize<List<ItemModifierGroupListViewModel>>(modifierItemList);
        }

        if (!ModelState.IsValid)
        {
            model.CategoryList = _categoryItemService.GetCategories();
            model.ItemTypeList = _categoryItemService.GetItemtypes();
            model.UnitList = _categoryItemService.GetUnits();
            model.ModifierGropList = _modifiersService.GetAllModifierGroup();
            model.ItemModifierList = await _modifiersService.GetAllModifierItemById(model.ItemId);
            return PartialView("_addItemModalPartialView", model);
        }

        string result = "";
        bool isCreated = true;

        //For Adding New Item
        if (model.ItemId == 0)
        {
            result = await _categoryItemService.AddItem(model, userId);
        }
        //For Editing Item
        else
        {
            result = await _categoryItemService.EditItem(model, userId);
            isCreated = false;
        }
        // Checking for Add or Update
        if (result.Equals("true"))
        {
            TempData["NotificationMessage"] = string.Format(isCreated ? NotificationMessages.EntityCreated : NotificationMessages.EntityUpdated, "Item");
            TempData["NotificationType"] = NotificationType.Success.ToString();
            return Json(new { success = true });
        }
        else
        {
            TempData["NotificationMessage"] = result;
            TempData["NotificationType"] = NotificationType.Error.ToString();
            return PartialView("_addItemModalPartialView", model);
        }
    }
    #endregion

    #region Delete Item
    [HttpPost]
    [PermissionAuthorize("Menu_Delete")]
    public async Task<IActionResult> DeleteItem(long id)
    {
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);

        bool result = await _categoryItemService.DeleteItem(id, userName);
        if (result)
        {
            return Json(new { success = true, message = string.Format(NotificationMessages.EntityDeleted, "Item") });
        }
        else
        {
            return Json(new { success = false, message = string.Format(NotificationMessages.EntityDeletedFailed, "Item") });
        }
    }

    
    #endregion
    
    #region Mass Delte Item
    [HttpPost]
    [PermissionAuthorize("Menu_Delete")]
    public async Task<IActionResult> DeleteSelectedItems(List<long> id)
    {
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);

        bool result = await _categoryItemService.DeleteSelectedItems(id, userName);
        if (result)
        {
            return Json(new { success = true, message = string.Format(NotificationMessages.EntityDeleted, "Items") });
        }
        else
        {
            return Json(new { success = false, message = string.Format(NotificationMessages.EntityDeletedFailed, "Items") });
        }
    }
    #endregion
    
    // -------------------------------------------------------------------- Add Item Modifier Select Group ------------------------------------------------------------------//
    #region Modifier Item For Add Item
    [HttpGet]
    public async Task<IActionResult> GetModifierItemById(long modifierGroupId)
    {
        return PartialView("_modifierItemPartialView", await _modifiersService.GetModifierItemById(modifierGroupId));
    }
    #endregion

    /*-----------------------------------------------------------------------Modifier Group CRUD------------------------------------------------------------------------------*/

    #region Get Modifier Group
    public IActionResult ModifiersTab()
    {
        IEnumerable<ModifierGroupViewModel> model = _modifiersService.GetAllModifierGroup();
        return PartialView("_modifiersTab", model);
    }
    #endregion

    #region Add/Edit Modifier Group
    [HttpGet]
    // [PermissionAuthorize("Menu_AddEdit")]
    public async Task<IActionResult> SaveModifierGroup(long id)
    {
        //for the Add Item Model
        ModifierGroupViewModel model = new ModifierGroupViewModel();

        //Fetch the Item details for Edit Item Modal
        if (id > 0)
        {
            model = await _modifiersService.GetModifierGroupById(id);
        }

        return PartialView("_modifierGroupAdd", model);
    }

    [HttpPost]
    [PermissionAuthorize("Menu_AddEdit")]
    public async Task<IActionResult> SaveModifierGroup(ModifierGroupViewModel model, string modifierItemList)
    {
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);
        long userId = await _userDetailService.GetUserIdByUserNameAsync(userName);
        if (!string.IsNullOrEmpty(modifierItemList))
        {
            model.ModifierItemList = JsonSerializer.Deserialize<List<ModifierItemViewModel>>(modifierItemList);
        }
        if (!ModelState.IsValid)
        {
            model.ModifierItemList = JsonSerializer.Deserialize<List<ModifierItemViewModel>>(modifierItemList);
            return PartialView("_modifierGroupAdd", model);
        }

        string result = "";
        bool isCreated = true;

        //For Adding New Category
        if (model.ModifierId == 0)
        {
            result = await _modifiersService.AddModifierGroup(model, userId);
        }
        //For Editing Category
        else
        {
            result = await _modifiersService.EditModifierGroup(model, userId);
            isCreated = false;
        }
        // Checking for Add or Update
        if (result.Equals("true"))
        {
            TempData["NotificationMessage"] = string.Format(isCreated ? NotificationMessages.EntityCreated : NotificationMessages.EntityUpdated, "Modifier Group");
            TempData["NotificationType"] = NotificationType.Success.ToString();
            return Json(new { success = true });
        }
        else
        {
            TempData["NotificationMessage"] = result;
            TempData["NotificationType"] = NotificationType.Error.ToString();
            return PartialView("_modifierGroupAdd", model);
        }
    }
    #endregion

    #region Delete Modifier Group
    [HttpPost]
    [PermissionAuthorize("Menu_Delete")]
    public async Task<IActionResult> DeleteModifierGroup(long id)
    {
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);

        bool result = await _modifiersService.DeleteModifierGroup(id, userName);
        if (result)
        {
            return Json(new { success = true, message = string.Format(NotificationMessages.EntityDeleted, "Modifier Group") });
        }
        else
        {
            return Json(new { success = false, message = string.Format(NotificationMessages.EntityDeletedFailed, "Modifier group") });
        }
    }
    #endregion

    /*---------------------------------------------------------------------------Modifier Item CRUD------------------------------------------------------------------------------*/

    #region Get Modifer Items
    public async Task<IActionResult> GetModifierItems(long modifierGroupId = 1, int pageNo = 1, int pageSize = 3, string search = "")
    {
        return PartialView("_ModifierItemsPartialView", await _modifiersService.GetModfierItems(modifierGroupId, pageNo, pageSize, search));
    }

    public async Task<IActionResult> GetAllModifierItems(int pageNo = 1, int pageSize = 3, string search = "")
    {
        return PartialView("_existingModifierList", await _modifiersService.GetAllModfierItems(pageNo, pageSize, search));
    }
    #endregion
    
    #region Add/Edit Modifer Item
    [HttpGet]
    // [PermissionAuthorize("Menu_AddEdit")]
    public async Task<IActionResult> SaveModifier(long id)
    {
        //for the Add Item Model
        AddEditModifierViewModel model = new AddEditModifierViewModel();

        //Fetch the Item details for Edit Item Modal
        if (id > 0)
        {
            model = await _modifiersService.GetModifierById(id);
        }
        model.UnitList = _categoryItemService.GetUnits();
        model.ModifierGroupList = _modifiersService.GetAllModifierGroup();

        return PartialView("_addModifierModal", model);
    }


    [HttpPost]
    [PermissionAuthorize("Menu_AddEdit")]
    public async Task<IActionResult> SaveModifier(AddEditModifierViewModel model)
    {
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);
        long userId = await _userDetailService.GetUserIdByUserNameAsync(userName);
        if (!ModelState.IsValid)
        {
            model.UnitList = _categoryItemService.GetUnits();
            model.ModifierGroupList = _modifiersService.GetAllModifierGroup();
            return PartialView("_addModifierModal", model);   
        }

        string result = "";
        bool isCreated = true;

        //For Adding New Category
        if (model.ModifierItemId == 0)
        {
            result = await _modifiersService.AddModifierItem(model, userId);
        }
        //For Editing Category
        else
        {
            result = await _modifiersService.EditModifierItem(model, userId);
            isCreated = false;
        }
        // Checking for Add or Update
        if (result.Equals("true"))
        {
            var message = string.Format(isCreated ? NotificationMessages.EntityCreated : NotificationMessages.EntityUpdated, "Modifier");
            return Json(new { success = true, message = message});
        }
        else
        {
           return Json(new { success = false, errorMessage = result });
        }
    }
    #endregion

    #region Delete Modifier Item
    [HttpPost]
    [PermissionAuthorize("Menu_Delete")]
    public async Task<IActionResult> DeleteModifier(long id)
    {
        string? token = HttpContext.Session.GetString("SuperSecretAuthToken");
        string userName = _userDetailService.UserName(token);

        bool result = await _modifiersService.DeleteModifierItem(id, userName);
        if (result)
        {
            return Json(new { success = true, message = string.Format(NotificationMessages.EntityDeleted, "Modifier") });
        }
        else
        {
            return Json(new { success = false, message = string.Format(NotificationMessages.EntityDeletedFailed, "Modifier") });
        }
    }
    #endregion
}
