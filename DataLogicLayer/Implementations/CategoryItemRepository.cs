using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DataLogicLayer.Implementations;

public class CategoryItemRepository : ICategoryItemRepository
{
    private readonly PizzaShopDbContext _context;


    public CategoryItemRepository(PizzaShopDbContext context)
    {
        _context = context;

    }

    public List<Category> GetAllCategories()
    {
        return _context.Categories.Where(c => !c.Isdeleted).ToList();
    }
    public async Task<bool> AddCategoryAsync(CategoryViewModel model, long userId)
    {
        try
        {
            Category newCategory = new Category
            {
                Name = model.CategoryName,
                Description = model.Description,
                CreatedBy = userId
            };

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in Adding Category : ", ex.Message);
            return false;
        }
    }

    public CategoryViewModel GetCategoryId(long id)
    {
        return _context.Categories.Where(c => c.CategoryId == id && !c.Isdeleted).Select(c => new CategoryViewModel
        {
            Id = c.CategoryId,
            CategoryName = c.Name,
            Description = c.Description
        }).FirstOrDefault();
    }

    public async Task<bool> EditCategoryAsync(CategoryViewModel model, long userId)
    {
        try
        {
            if (model.Id == null || model.Id == -1)
            {
                return false;
            }

            Category category = await _context.Categories.Where(c => c.CategoryId == model.Id && !c.Isdeleted).FirstOrDefaultAsync();

            if (category == null) return false;

            category.Name = model.CategoryName;
            category.Description = model.Description;
            category.UpdatedAt = DateTime.Now;
            category.UpdatedBy = userId;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Updating Category", ex.Message);
            return false;
        }
    }

    public async Task<bool> DeleteCategoryAsync(long categoryId, long userId)
    {
        Category category = _context.Categories.Where(c => c.CategoryId == categoryId && !c.Isdeleted).FirstOrDefault();
        if(category == null)
        {
            return false;
        }
        try
        {
            category.Isdeleted = true;
            category.UpdatedBy = userId;
            category.UpdatedAt = DateTime.Now;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Deleting User", ex.Message);
            return false;
        }
    }
}
