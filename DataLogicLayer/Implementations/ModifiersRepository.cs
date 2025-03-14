using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DataLogicLayer.Implementations;

public class ModifiersRepository : IModifiersRepository
{
    private readonly PizzaShopDbContext _context;


    public ModifiersRepository(PizzaShopDbContext context)
    {
        _context = context;

    }

    /*---------------------------------------------------------------------------Modifier Group CRUD------------------------------------------------------------------------------*/

    #region Get Modifier Group
    public IEnumerable<Modifiergroup> GetAllModifierGroup()
    {
        return _context.Modifiergroups.Where(m => !m.Isdeleted).ToList();
    }
    #endregion

    #region ADD : Modifier Group
    public async Task<string> AddModifierAsync(ModifierGroupViewModel model, long userId)
    {
        Modifiergroup existingGroup = await _context.Modifiergroups.Where(m => m.Name == model.Name && !m.Isdeleted && m.Id != model.ModifierId).FirstOrDefaultAsync();
        if (existingGroup != null && existingGroup.Isdeleted == false)
        {
            return $"{model.Name} Modifier already exist! ";
        }
        if (existingGroup != null && existingGroup.Isdeleted == true)
        {
            existingGroup.Name = string.Concat(existingGroup.Name, DateTime.Now);
            _context.Modifiergroups.Update(existingGroup);
            await _context.SaveChangesAsync();
        }

        try
        {
            Modifiergroup modifiergroup = new Modifiergroup
            {
                Name = model.Name,
                Description = model.Description,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            _context.Modifiergroups.Add(modifiergroup);
            await _context.SaveChangesAsync();

            if (await AddModifierItem(modifiergroup.Id, model.ModifierItemList, userId))
            {
                return "true";
            }
            return "Failed To Add Modifier";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error In Category Repository :", ex.Message);
            return "Error Adding Modifier";
        }
    }
    #endregion

    #region EDIT : Modifier Group
    public async Task<string> EditModifierAsync(ModifierGroupViewModel model, long userId)
    {
        Modifiergroup? existingGroup = await _context.Modifiergroups.Where(m => m.Name == model.Name && !m.Isdeleted && m.Id != model.ModifierId).FirstOrDefaultAsync();
        if (existingGroup != null && existingGroup.Isdeleted == false)
        {
            return $"{model.Name} Modifier already exist! ";
        }
        if (existingGroup != null && existingGroup.Isdeleted == true)
        {
            existingGroup.Name = string.Concat(existingGroup.Name, DateTime.Now);
            _context.Modifiergroups.Update(existingGroup);
            await _context.SaveChangesAsync();
        }

        try
        {
            Modifiergroup? modifiergroup = _context.Modifiergroups.FirstOrDefault(m => m.Id == model.ModifierId);
            if (modifiergroup == null)
            {
                return "Modifier not found!";
            }

            modifiergroup.Name = model.Name;
            modifiergroup.Description = model.Description;
            modifiergroup.UpdatedAt = DateTime.Now;
            modifiergroup.UpdatedBy = userId;

            _context.Modifiergroups.Update(modifiergroup);
            await _context.SaveChangesAsync();

            if (await AddModifierItem(modifiergroup.Id, model.ModifierItemList, userId))
            {
                return "true";
            }
            return "Failed To Edit Modifier";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error In Category Repository :", ex.Message);
            return "Error Adding Item";
        }
    }
    #endregion

    #region DELETE : Modifier Group
    public async Task<bool> DeleteModifierGroupAsync(long modifierGroupId, long userId)
    {
        Modifiergroup? modifiergroup = _context.Modifiergroups.Where(c => c.Id != modifierGroupId && !c.Isdeleted).FirstOrDefault();
        List<Modifiergroupitemmap> modifierGroupItems = _context.Modifiergroupitemmaps.Where(i => i.ModifierGroupId == modifierGroupId).ToList();
        if (modifiergroup == null)
        {
            return false;
        }
        try
        {
            //For Cascade Soft Deleting the Items with Category
            if (modifierGroupItems != null)
            {
                foreach (var item in modifierGroupItems)
                {
                    item.Isdeleted = true;
                    item.UpdatedAt = DateTime.Now;
                    item.UpdatedBy = userId;
                    _context.Modifiergroupitemmaps.Update(item);
                    await _context.SaveChangesAsync();
                }
            }
            modifiergroup.Isdeleted = true;
            modifiergroup.UpdatedBy = userId;
            modifiergroup.UpdatedAt = DateTime.Now;
            _context.Modifiergroups.Update(modifiergroup);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Deleting Modifier Group", ex.Message);
            return false;
        }
    }
    #endregion

    /*---------------------------------------------------------------------------Modifier Item CRUD------------------------------------------------------------------------------*/

    #region Get Modifier Items for modifier Id
    public async Task<AddEditModifierViewModel> GetModifierByIdAsync(long modifierId)
    {
        AddEditModifierViewModel? model = await _context.Modifieritems.Include(mi => mi.ModifierGroup)
                                        .Where(mi => mi.Id == modifierId && !mi.Isdeleted)
                                        .Select(mi => new AddEditModifierViewModel
                                        {
                                            ModifierGroupId = mi.ModifierGroup.Id,
                                            OldModifierGroupId = mi.ModifierGroup.Id,
                                            ModifierItemId = modifierId,
                                            Name = mi.Name,
                                            Description = mi.Description,
                                            Rate = mi.Rate,
                                            Quantity = mi.Quantity,
                                            UnitId = mi.Unitid,
                                        }).FirstOrDefaultAsync();


        return model;
    }
    #endregion

    #region Get Modifier Group By Modifier Id
    public async Task<ModifierGroupViewModel> GetModifierGroupByIdAsync(long modifierId)
    {
        ModifierGroupViewModel? model = await _context.Modifiergroups
                                         .Include(mg => mg.Modifiergroupitemmaps)
                                         .ThenInclude(mi => mi.ModifierItem)
                                         .Where(mg => mg.Id == modifierId && !mg.Isdeleted)
                                         .Select(mg => new ModifierGroupViewModel
                                         {
                                             ModifierId = mg.Id,
                                             Name = mg.Name,
                                             Description = mg.Description,
                                             ModifierItemList = mg.Modifiergroupitemmaps
                                             .Where(m => m.ModifierGroupId == modifierId && !m.Isdeleted && !m.ModifierItem.Isdeleted)
                                             .Select(m => new ModifierItemViewModel
                                             {
                                                 ModifierItemId = m.ModifierItem.Id,
                                                 Name = m.ModifierItem.Name,
                                                 Unit = m.ModifierItem.Unit.Name,
                                                 Rate = m.ModifierItem.Rate,
                                                 Quantity = m.ModifierItem.Quantity,
                                             }).ToList()
                                         }).FirstOrDefaultAsync();


        return model;
    }
    #endregion

    #region Get Modifier Items By Group Id
    public async Task<(IEnumerable<ModifierItemViewModel> modifierItems, int totalRecords)> GetModifierItemAsync(long modifierGroupId, int pageNo, int pageSize, string search)
    {
        IQueryable<ModifierItemViewModel> query = _context.Modifiergroupitemmaps
                                .Include(mi => mi.ModifierItem)
                                .Where(m => m.ModifierGroupId == modifierGroupId && !m.Isdeleted && !m.ModifierItem.Isdeleted)
                                .Select(m => new ModifierItemViewModel
                                {
                                    ModifierItemId = m.ModifierItem.Id,
                                    Name = m.ModifierItem.Name,
                                    Rate = m.ModifierItem.Rate,
                                    Unit = m.ModifierItem.Unit.Name,
                                    Quantity = m.ModifierItem.Quantity,
                                });

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(m => m.Name.ToLower().Contains(search) ||
                                m.Rate.ToString().Contains(search) ||
                                m.Quantity.ToString().Contains(search));
        }

        int totalRecords = await query.CountAsync();

        IEnumerable<ModifierItemViewModel> modifierItems = await query
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

        return (modifierItems, totalRecords);
    }
    #endregion

    #region Modifier Item for Add Item Modal
    public async Task<List<ItemModifierGroupListViewModel>> GetModifierItemByItemId(long itemId)
    {
        List<ItemModifierGroupListViewModel> model = await _context.Itemmodifiergroups.
        Where(i => i.Itemid == itemId && !i.Isdeleted).Select(i => new ItemModifierGroupListViewModel
        {
            ItemId = i.Itemid,
            ModifierGroupId = i.ModifierGroupId,
            Name = i.ModifierGroup.Name,
            MinAllowed = i.MinAllowed,
            MaxAllowed = i.MaxAllowed,
            ModifierItemList = i.ModifierGroup.Modifieritems.Select(i => new ModifierItemViewModel
            {
                ModifierItemId = i.Id,
                Name = i.Name,
                Rate = i.Rate,
                Quantity = i.Quantity
            }).ToList()
        }).ToListAsync();
        return model;
    }
    #endregion

    #region ADD : Modifier Item
    public async Task<string> AddModifierItemAsync(AddEditModifierViewModel model, long userId)
    {
        Modifieritem? existingModifier = await _context.Modifieritems.Where(mi => mi.Name == model.Name && mi.Id != model.ModifierItemId && !mi.Isdeleted).FirstOrDefaultAsync();
        if (existingModifier != null && existingModifier.Isdeleted == false)
        {
            return $"{model.Name} Modifier already exist! ";
        }
        if (existingModifier != null && existingModifier.Isdeleted == true)
        {
            existingModifier.Name = string.Concat(existingModifier.Name, DateTime.Now);
            _context.Modifieritems.Update(existingModifier);
            await _context.SaveChangesAsync();
        }

        try
        {
            Modifieritem modifierItem = new Modifieritem
            {
                ModifierGroupId = model.ModifierGroupId,
                Name = model.Name,
                Description = model.Description,
                Rate = model.Rate,
                Quantity = model.Quantity,
                Unitid = model.UnitId,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            _context.Modifieritems.Add(modifierItem);
            await _context.SaveChangesAsync();

            await SaveModifierItem(modifierItem.Id, model, userId);
            return "true";

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error In Category Repository :", ex.Message);
            return "Error Adding Modifier";
        }
    }
    #endregion

    #region EDIT : Modifier Item
    public async Task<string> EditModifierItemAsync(AddEditModifierViewModel model, long userId)
    {
        Modifieritem? existingModifier = await _context.Modifieritems.Where(mi => mi.Name == model.Name && mi.Id != model.ModifierItemId && !mi.Isdeleted).FirstOrDefaultAsync();
        if (existingModifier != null && existingModifier.Isdeleted == false)
        {
            return $"{model.Name} Modifier already exist! ";
        }

        if (existingModifier != null && existingModifier.Isdeleted == true)
        {
            existingModifier.Name = string.Concat(existingModifier.Name, DateTime.Now);
            _context.Modifieritems.Update(existingModifier);
            await _context.SaveChangesAsync();
        }

        try
        {
            Modifieritem? modifierItem = await _context.Modifieritems.Where(mi => mi.Id == model.ModifierItemId && !mi.Isdeleted).FirstOrDefaultAsync();
            if (modifierItem == null)
            {
                return "Modifier not found!";
            }

            modifierItem.Name = model.Name;
            modifierItem.Description = model.Description;
            modifierItem.Rate = model.Rate;
            modifierItem.Quantity = model.Quantity;
            modifierItem.Unitid = model.UnitId;
            modifierItem.ModifierGroupId = model.ModifierGroupId;
            modifierItem.UpdatedAt = DateTime.Now;
            modifierItem.UpdatedBy = userId;

            _context.Modifieritems.Update(modifierItem);
            await _context.SaveChangesAsync();

            //check if new modifier group is different
            if (!model.ModifierGroupId.Equals(model.OldModifierGroupId))
            {
                await SaveModifierItem(modifierItem.Id, model, userId);

            }
            return "true";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error In Category Repository :", ex.Message);
            return "Error Adding Item";
        }
    }
    #endregion

    #region DELETE : Modifier Item
    public async Task<bool> DeleteModifierItemAsync(long modifierId, long userId)
    {
        Modifieritem? modifierItem = _context.Modifieritems.Where(mi => mi.Id == modifierId && !mi.Isdeleted).FirstOrDefault();
        List<Modifiergroupitemmap> modifierGroupItems = _context.Modifiergroupitemmaps.Where(i => i.ModifierItemId == modifierId && !i.Isdeleted).ToList();
        if (modifierItem == null)
        {
            return false;
        }
        try
        {
            //For Cascade Soft Deleting the Items with Category
            if (modifierGroupItems != null)
            {
                foreach (var item in modifierGroupItems)
                {
                    item.Isdeleted = true;
                    item.UpdatedAt = DateTime.Now;
                    item.UpdatedBy = userId;
                    _context.Modifiergroupitemmaps.Update(item);
                    await _context.SaveChangesAsync();
                }
            }
            modifierItem.Isdeleted = true;
            modifierItem.UpdatedBy = userId;
            modifierItem.UpdatedAt = DateTime.Now;
            _context.Modifieritems.Update(modifierItem);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Deleting Modifier", ex.Message);
            return false;
        }
    }
    #endregion

    /*---------------------------------------------------------------------------Existing Modifiers CRUD------------------------------------------------------------------------------*/

    #region Save Modifier Items In Mapping Table
    public async Task SaveModifierItem(long modifierId, AddEditModifierViewModel model, long userId)
    {
        if (model.OldModifierGroupId != 0)
        {
            Modifiergroupitemmap? oldModifier = await _context.Modifiergroupitemmaps
                                            .Where(im => im.ModifierGroupId == model.OldModifierGroupId && im.ModifierItemId == modifierId && !im.Isdeleted)
                                            .FirstOrDefaultAsync();

            if (oldModifier != null)
            {

                //Delete the existing Modifer and Group Mapping
                oldModifier.Isdeleted = true;
                oldModifier.UpdatedBy = userId;
                oldModifier.UpdatedAt = DateTime.Now;
                _context.Modifiergroupitemmaps.Update(oldModifier);
                await _context.SaveChangesAsync();
            }
        }
        Modifiergroupitemmap? existingOne = await _context.Modifiergroupitemmaps
                                            .Where(im => im.ModifierGroupId == model.ModifierGroupId && im.ModifierItemId == modifierId && !im.Isdeleted)
                                            .FirstOrDefaultAsync();

        if (existingOne != null)
        {
            existingOne.UpdatedBy = userId;
            existingOne.UpdatedAt = DateTime.Now;
            _context.Modifiergroupitemmaps.Update(existingOne);
            await _context.SaveChangesAsync();
            return;
        }

        //If Existing Item is Not Found Add It to Items Mapping
        Modifiergroupitemmap modifiergroupitemmap = new Modifiergroupitemmap
        {
            ModifierGroupId = model.ModifierGroupId,
            ModifierItemId = modifierId,
            CreatedBy = userId,
            CreatedAt = DateTime.Now,

        };
        _context.Modifiergroupitemmaps.Add(modifiergroupitemmap);
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Delete Modifier Items In Mapping Table
    public async Task<bool> DeleteExistingModifiers(long id, long userId)
    {
        try
        {
            Modifiergroupitemmap? existingOne = await _context.Modifiergroupitemmaps
                .Where(mi => mi.ModifierItemId == id && !mi.Isdeleted)
                .FirstOrDefaultAsync();

            if (existingOne == null)
            {
                return false;
            }
            existingOne.Isdeleted = true;
            existingOne.UpdatedAt = DateTime.Now;
            existingOne.UpdatedBy = userId;

            _context.Modifiergroupitemmaps.Update(existingOne);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Deleting Selected Modifier Item", ex.Message);
            return false;
        }
    }
    #endregion

    #region ADD : Existing Modifiers
    public async Task<bool> AddModifierItem(long modifierGroupId, List<ModifierItemViewModel> ModifierItemList, long userId)
    {
        List<long> existingModifierIds = await _context.Modifiergroupitemmaps.Where(mi => mi.ModifierGroupId == modifierGroupId && !mi.Isdeleted)
                                        .Select(mi => mi.ModifierItemId).ToListAsync();

        List<long> newModifierIds = ModifierItemList.Select(m => m.ModifierItemId).ToList();
        List<long> toBeRemoved = existingModifierIds.Except(newModifierIds).ToList();

        try
        {
            if (toBeRemoved.Count > 0)
            {
                foreach (var deleteItem in toBeRemoved)
                {
                    await DeleteExistingModifiers(deleteItem, userId);
                }
            }

            foreach (var item in ModifierItemList)
            {
                Modifiergroupitemmap? existingOne = await _context.Modifiergroupitemmaps
                .Where(mi => mi.ModifierGroupId == modifierGroupId && mi.ModifierItemId == item.ModifierItemId && !mi.Isdeleted)
                .FirstOrDefaultAsync();

                if (existingOne != null)
                {
                    continue;
                }
                Modifiergroupitemmap newModifierItemMap = new Modifiergroupitemmap
                {
                    ModifierGroupId = modifierGroupId,
                    ModifierItemId = item.ModifierItemId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId,
                };

                await _context.Modifiergroupitemmaps.AddAsync(newModifierItemMap);
                await _context.SaveChangesAsync();
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

    #region Get : Modifier Items For Modal Pagination
    public async Task<(IEnumerable<ModifierItemViewModel> modifierItems, int totalRecords)> GetAllModifierItemAsync(int pageNo, int pageSize, string search)
    {
        IQueryable<ModifierItemViewModel> query = _context.Modifieritems
                                 .Include(m => m.Unit)
                                 .Where(m => !m.Isdeleted)
                                 .Select(m => new ModifierItemViewModel
                                 {
                                     ModifierItemId = m.Id,
                                     Name = m.Name,
                                     Rate = m.Rate,
                                     Unit = m.Unit.Name,
                                     Quantity = m.Quantity,
                                 });

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(m => m.Name.ToLower().Contains(search) ||
                                m.Rate.ToString().Contains(search) ||
                                m.Quantity.ToString().Contains(search));
        }

        int totalRecords = await query.CountAsync();

        IEnumerable<ModifierItemViewModel> modifierItems = await query
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

        return (modifierItems, totalRecords);
    }
    #endregion

    #region Get : Modifier Of Existing Modifiers
    public async Task<ModifierGroupViewModel> GetModifierItemByIdAsync(long modifierId)
    {
        ModifierGroupViewModel? modifiergroup = await _context.Modifiergroups.Where(m => m.Id == modifierId).
                                    Include(m => m.Modifieritems).
                                    Select(m => new ModifierGroupViewModel
                                    {
                                        ModifierId = m.Id,
                                        Name = m.Name,
                                        Description = m.Description,
                                        ModifierItemList = m.Modifieritems.Select(i => new ModifierItemViewModel
                                        {
                                            ModifierItemId = i.Id,
                                            Name = i.Name,
                                            Rate = i.Rate,
                                            Quantity = i.Quantity,
                                        }).ToList()
                                    }).FirstOrDefaultAsync();

        return modifiergroup;
    }
    #endregion
}


