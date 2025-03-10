using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DataLogicLayer.Interfaces;

public interface ICategoryItemRepository
{
    public List<Category> GetAllCategories();
    public CategoryViewModel GetCategoryId(long id);
    public Task<(List<ItemsViewModel> items, int totalRecords)> GetItemList(long categoryId, int pageNo, int pageSize, string search);


    #region Get Item Type And Unit
    public List<Itemtype> GetAllItemType();
    public List<Unit> GetAllUnit();
    #endregion

    #region CRUD for Category
    public Task<bool> AddCategoryAsync(CategoryViewModel model, long userId);
    public Task<bool> EditCategoryAsync(CategoryViewModel model, long userId);
    public Task<bool> DeleteCategoryAsync(long categoryId, long userId);
    #endregion

    #region CRUD for Items
    public Task<string> AddItemAsync(AdditemViewModel model, long userId);
    public Task<bool> AddItemModifier(long itemId, List<ItemModifierGroupListViewModel> itemModifierList, long userId);
    public Task<bool> EditItemModifier(long itemId, List<ItemModifierGroupListViewModel> itemModifierList, long userId);

    public Task<string> EditItemAsync(AdditemViewModel model, long userId);
    public Task<Item> GetItemByIdAsync(long id);

    public Task<bool> DeleteItemAsync(long id, long userId);
    #endregion
}
