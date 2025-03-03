using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;

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
}
