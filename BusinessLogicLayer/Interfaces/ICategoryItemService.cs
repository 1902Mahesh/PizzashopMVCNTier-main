using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLogicLayer.Interfaces;

public interface ICategoryItemService
{
    public List<CategoryViewModel> GetCategories();
    public CategoryViewModel GetCategoryById(long id);
    public Task<ItemListViewModel> GetItemList(long categoryId,int pageNo, int pageSize, string search);
    public List<Itemtype> GetItemtypes();
    public List<Unit> GetUnits();
    public Task<bool> AddCategory(CategoryViewModel model, string userName);
    public Task<bool> EditCategory(CategoryViewModel model, string userName);
    public Task<bool> DeleteCategory(long categoryId, string userName);

    #region CRUD For Items

    public Task<AdditemViewModel> GetItemByID(long id);

    public Task<string> AddItem(AdditemViewModel model, long userId);
    public Task<string> EditItem(AdditemViewModel model, long userId);


    public Task<bool> DeleteItem(long id, string userName);
    public Task<bool> DeleteSelectedItems(List<long> id, string userName);
    #endregion
}
