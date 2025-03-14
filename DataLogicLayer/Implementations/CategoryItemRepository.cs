using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataLogicLayer.Implementations;

public class CategoryItemRepository : ICategoryItemRepository
{
    private readonly PizzaShopDbContext _context;


    public CategoryItemRepository(PizzaShopDbContext context)
    {
        _context = context;

    }

    /*---------------------------------------------------------------------------Items, Unit List------------------------------------------------------------------------------*/

    #region Get Item, Unit and Item Type
    public List<Itemtype> GetAllItemType()
    {
        return _context.Itemtypes.ToList();
    }

    public List<Unit> GetAllUnit()
    {
        return _context.Units.ToList();
    }

    #endregion
    /*---------------------------------------------------------------------------Category CRUD------------------------------------------------------------------------------*/
    
    #region Get Categories
    public List<Category> GetAllCategories()
    {
        return _context.Categories.Where(c => !c.Isdeleted).ToList();
    }

    public async Task<(List<ItemsViewModel> items, int totalRecords)> GetItemList(long categoryId, int pageNo, int pageSize, string search)
    {
        IQueryable<ItemsViewModel> query = _context.Items
                            .Include(i => i.Itemtype)
                            .Where(i => i.Categoryid == categoryId && !i.Isdeleted)
                            .Select(i => new ItemsViewModel
                            {
                                ItemId = i.ItemId,
                                ItemName = i.Name,
                                ItemType = i.Itemtype.Imgurl,
                                Rate = i.Rate,
                                Quantity = i.Quantity,
                                ItemImg = i.Imgurl,
                                IsAvailable = i.Isavailable
                            });


        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(i => i.ItemName.ToLower().Contains(search) ||
                                i.Rate.ToString().Contains(search) ||
                                i.Quantity.ToString().Contains(search));
        }

        int totalRecords = await query.CountAsync();

        List<ItemsViewModel> items = await query
                     .Skip((pageNo - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync();

        return (items, totalRecords);
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
    #endregion

    #region ADD : Category
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
    #endregion

    #region EDIT : Category
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
    #endregion

    #region DELETE : Category
    public async Task<bool> DeleteCategoryAsync(long categoryId, long userId)
    {
        Category category = _context.Categories.Where(c => c.CategoryId == categoryId && !c.Isdeleted).FirstOrDefault();
        List<Item> items = _context.Items.Where(i => i.Categoryid == categoryId).ToList();
        if (category == null)
        {
            return false;
        }
        try
        {
            //For Cascade Soft Deleting the Items with Category
            if (items != null)
            {
                foreach (var item in items)
                {
                    item.Isdeleted = true;
                    item.UpdatedAt = DateTime.Now;
                    item.UpdatedBy = userId;
                    _context.Items.Update(item);
                    await _context.SaveChangesAsync();
                }
            }
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
    #endregion

    /*---------------------------------------------------------------------------Items CRUD------------------------------------------------------------------------------*/

    #region Get Items
    public async Task<Item> GetItemByIdAsync(long id)
    {
        Item? item = await _context.Items.Where(i => i.ItemId == id && !i.Isdeleted).FirstOrDefaultAsync();
        return item;    
    }
    #endregion

    #region ADD : Items
    public async Task<string> AddItemAsync(AdditemViewModel model, long userId)
    {
        Item? oldItem = await _context.Items.Where(i => i.Name == model.Name && !i.Isdeleted && i.ItemId != model.ItemId).FirstOrDefaultAsync();
        // Taxis tax = new
        if (oldItem != null && oldItem.Isdeleted == false)
        {
            return $"{model.Name} Item already exist! ";
        }
        if (oldItem != null && oldItem.Isdeleted == true)
        {
            oldItem.Name = string.Concat(oldItem.Name, DateTime.Now);
            _context.Items.Update(oldItem);
            await _context.SaveChangesAsync();
        }

        try
        {
            Item item = new Item
            {
                Name = model.Name,
                Description = model.Description,
                ItemtypeId = model.ItemTypeId,
                Rate = model.Rate,
                Unitid = model.UnitId,
                Quantity = model.Quantity,
                Isavailable = model.IsAvailable,
                DefaultTax = model.DefaultTax,
                AdditionalTax = model.TaxPercentage,
                Shortcode = model.ShortCode,
                Categoryid = model.CategoryId,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                Taxid = 1,
            };

            // Handle Image Upload
            if (model.ItemImage != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = $"{Guid.NewGuid()}_{model.ItemImage.FileName}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ItemImage.CopyToAsync(fileStream);
                }

                item.Imgurl = $"/uploads/{fileName}"; // Store relative path in DB
            }

            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();

            if(await AddItemModifier(item.ItemId, model.ItemModifierList, userId))
            {
                return "true";
            }
            return "Failed To Add Item";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error In Category Repository :", ex.Message);
            return "Error Adding Item";
        }
    }
    #endregion

    #region EDIT : Items
    public async Task<string> EditItemAsync(AdditemViewModel model, long userId)
    {
        Item? item = await _context.Items.Where(i => i.ItemId == model.ItemId).FirstOrDefaultAsync();
        Item? oldItem = await _context.Items.Where(i => i.Name == model.Name && i.ItemId != model.ItemId).FirstOrDefaultAsync();

        if (item == null)
        {
            return "Item Doest not Exist";
        }

        if (oldItem != null && oldItem.Isdeleted == false)
        {
            return $"{model.Name} Item already exist! ";
        }

        if (oldItem != null && oldItem.Isdeleted == true)
        {
            oldItem.Name = string.Concat(oldItem.Name, DateTime.Now);
            _context.Items.Update(oldItem);
            await _context.SaveChangesAsync();
        }

        try
        {
            item.Name = model.Name;
            item.Description = model.Description;
            item.ItemtypeId = model.ItemTypeId;
            item.Rate = model.Rate;
            item.Unitid = model.UnitId;
            item.Quantity = model.Quantity;
            item.Isavailable = model.IsAvailable;
            item.DefaultTax = model.DefaultTax;
            item.AdditionalTax = model.TaxPercentage;
            item.Shortcode = model.ShortCode;
            item.Categoryid = model.CategoryId;
            item.UpdatedBy = userId;
            item.UpdatedAt = DateTime.Now;

            // Handle Image Upload
            if (model.ItemImage != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = $"{Guid.NewGuid()}_{model.ItemImage.FileName}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ItemImage.CopyToAsync(fileStream);
                }

                item.Imgurl = $"/uploads/{fileName}"; // Store relative path in DB
            }

            _context.Items.Update(item);
            await _context.SaveChangesAsync();

            if(await AddItemModifier(item.ItemId, model.ItemModifierList, userId))
            {
                return "true";
            }
            return "Failed To Add Item";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error In Category Repository :", ex.Message);
            return "Error Updating Item";
        }
    }
    #endregion

    #region DELETE : Items
    public async Task<bool> DeleteItemAsync(long id, long userId)
    {
        Item item = await _context.Items.Where(i => i.ItemId == id).FirstOrDefaultAsync();
        if(item == null) return false;

        try
        {
            item.Isdeleted = true;
            item.UpdatedAt = DateTime.Now;
            item.UpdatedBy = userId;
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error In Delete item Repository: ", ex.Message);
            return false;
        }

    }
    #endregion

    /*---------------------------------------------------------------------------Add Item Modifiers CRUD------------------------------------------------------------------------------*/

    #region ADD : Modifers In Mapping Tabble
    public async Task<bool> AddItemModifier(long itemId, List<ItemModifierGroupListViewModel> itemModifierList, long userId)
    {
        List<long> exisingModifierIds = await _context.Itemmodifiergroups
                                        .Where(im => im.Itemid == itemId && !im.Isdeleted)
                                        .Select(mi => mi.ModifierGroupId)
                                        .ToListAsync();

        List<long> newModifierGroupIds = itemModifierList.Select(m => m.ModifierGroupId).ToList();
        List<long> toBeRemoved = exisingModifierIds.Except(newModifierGroupIds).ToList();

        try
        {
            if (toBeRemoved.Count > 0)
            {
                foreach (var deleteItem in toBeRemoved)
                {
                    await DeleteSelectModifierGroups(itemId,deleteItem, userId);
                }
            }
            
            foreach(var item in itemModifierList)
            {
                Itemmodifiergroup? existingOne = await _context.Itemmodifiergroups.Where(i => i.Itemid == itemId && i.ModifierGroupId == item.ModifierGroupId && !i.Isdeleted)
                .FirstOrDefaultAsync();

                if(existingOne != null)
                {
                    existingOne.MinAllowed = item.MinAllowed;
                    existingOne.MaxAllowed = item.MaxAllowed;
                    existingOne.UpdatedAt = DateTime.Now;
                    existingOne.UpdatedBy = userId;
                    _context.Itemmodifiergroups.Update(existingOne);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Itemmodifiergroup newItemModifer = new Itemmodifiergroup{
                        Itemid = itemId,
                        ModifierGroupId = item.ModifierGroupId,
                        MinAllowed = item.MinAllowed,
                        MaxAllowed = item.MaxAllowed,
                        CreatedAt = DateTime.Now,
                        CreatedBy = userId,
                    };

                    await _context.Itemmodifiergroups.AddAsync(newItemModifer);
                    await _context.SaveChangesAsync();
                }
            }
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in Modifier Item", ex.Message);
            return false;
        }
    }
    #endregion

    #region DELETE : Modifers In Mapping Tabble
    public async Task<bool> DeleteSelectModifierGroups(long Itemid,long id, long userId)
    {
        try
        {
            Itemmodifiergroup? existingOne = await _context.Itemmodifiergroups
                                            .Where(im => im.ModifierGroupId == id && im.Itemid == Itemid && !im.Isdeleted)
                                            .FirstOrDefaultAsync();

            if(existingOne == null)
            {
                return false;
            }

            existingOne.Isdeleted = true;
            existingOne.UpdatedAt = DateTime.Now;
            existingOne.UpdatedBy = userId;

            _context.Itemmodifiergroups.Update(existingOne);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Deleting Selected Modifier Group", ex.Message);
            return false;
        }
    }
    #endregion
}
