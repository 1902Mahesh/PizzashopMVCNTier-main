using BusinessLogicLayer.Common;
using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Digests;

namespace BusinessLogicLayer.Implementations;

public class CategoryItemService : ICategoryItemService
{
    private readonly ICategoryItemRepository _categoryItemRepository;
    private readonly IUserRepository _userRepository;

    public CategoryItemService(ICategoryItemRepository categoryItemRepository, IUserRepository userRepository)
    {
        _categoryItemRepository = categoryItemRepository;
        _userRepository = userRepository;
    }

    #region Add Category
    public async Task<bool> AddCategory(CategoryViewModel model, string userName)
    {
        User user = await _userRepository.GetUserByUserName(userName);
        if(user == null)
        {
            return false;
        }

        if(model == null)
        {
            return false;
        }

        return await _categoryItemRepository.AddCategoryAsync(model, user.Id);
    }

    #endregion

    #region Get Categories
    public List<CategoryViewModel> GetCategories()
    {
        List<Category> categories = _categoryItemRepository.GetAllCategories();
        return categories.Select( c => new CategoryViewModel {Id = c.CategoryId ,CategoryName = c.Name, Description = c.Description}).ToList();
    }

    public CategoryViewModel GetCategoryById(long id)
    {
        if(id == null || id == -1)
        {
            return new CategoryViewModel();
        }

        return _categoryItemRepository.GetCategoryId(id);
    }

    #endregion

    #region Edit Categories
    public async Task<bool> EditCategory(CategoryViewModel model, string userName)
    {

        User user = await _userRepository.GetUserByUserName(userName);
        if(user == null)
        {
            return false;
        }

        if(model == null)
        {
            return false;
        }
        return await _categoryItemRepository.EditCategoryAsync(model, user.Id);
    }

    #endregion

    #region Delete Category
    public async Task<bool> DeleteCategory(long categoryId, string userName)
    {
        if(categoryId == -1 || categoryId == null)
        {
            return false;
        }

        User user = await _userRepository.GetUserByUserName(userName);
        if(user == null)
        {
            return false;
        }

        return await _categoryItemRepository.DeleteCategoryAsync(categoryId, user.Id);
    }
    #endregion

    #region Get Items List
    public async Task<ItemListViewModel> GetItemList(long categoryId,int pageNo, int pageSize, string search)
    {
        ItemListViewModel model = new() {Page = new ()};
        var itemData = await _categoryItemRepository.GetItemList(categoryId, pageNo, pageSize, search);

        model.ItemList = itemData.items;
        model.Page.SetPagination(itemData.totalRecords, pageSize, pageNo);
        model.CategoryId = categoryId;
        return model;
    }

    public List<Itemtype> GetItemtypes()
    {
        return _categoryItemRepository.GetAllItemType();
    }

    public List<Unit> GetUnits()
    {
        return _categoryItemRepository.GetAllUnit();
    }

    #endregion

    #region Get Items
    public async Task<AdditemViewModel> GetItemByID(long id)
    {
        Item item = await _categoryItemRepository.GetItemByIdAsync(id);

        AdditemViewModel model = new AdditemViewModel();
        if(item != null)
        {
            model.ItemId = id;
            model.Name = item.Name;
            model.Description = item.Description;
            model.ItemTypeId = item.ItemtypeId;
            model.Rate = item.Rate;
            model.UnitId = item.Unitid;
            model.Quantity = item.Quantity;
            model.IsAvailable = item.Isavailable;
            model.TaxPercentage = item.AdditionalTax;
            model.DefaultTax = item.DefaultTax;
            model.ShortCode = item.Shortcode;
            model.CategoryId = item.Categoryid;
            model.ImgUrl = item.Imgurl;
        }

        return model;
    }
    #endregion

    #region ADD : Item
    public async Task<string> AddItem(AdditemViewModel model, long userId)
    {
        if(model == null)
        {
            return "Model is Empty";
        }
        return await _categoryItemRepository.AddItemAsync(model, userId);
    }
    #endregion

   #region EDIT : Item
    public async Task<string> EditItem(AdditemViewModel model, long userId)
    {
        if(model == null)
        {
            return "Model is Empty";
        }
        return await _categoryItemRepository.EditItemAsync(model, userId);
    }
    #endregion

    #region DELETE : Item
    public async Task<bool> DeleteItem(long id, string userName)
    {
        if(id == 0)
        {
            return false;
        }

        User user = await _userRepository.GetUserByUserName(userName);
        if(user == null)
        {
            return false;
        }

        return await _categoryItemRepository.DeleteItemAsync(id, user.Id);
    }
    #endregion

    #region DELETE : Mass Delete Items
    public async Task<bool> DeleteSelectedItems(List<long> id, string userName)
    {
        if(id.IsNullOrEmpty())
        {
            return false;
        }
        User user = await _userRepository.GetUserByUserName(userName);
        if(user == null)
        {
            return false;
        }

        bool result = true;

        foreach(long ItemId in id)
        {
            result = await _categoryItemRepository.DeleteItemAsync(ItemId, user.Id);
            if(result == false)
                return result;
        }
        return result;
    }
    #endregion
}
