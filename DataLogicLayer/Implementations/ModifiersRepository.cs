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
    public IEnumerable<Modifiergroup> GetAllModifierGroup()
    {
        return _context.Modifiergroups.Where(m => !m.Isdeleted).ToList();
    }

    public async Task<(IEnumerable<ModifierItemViewModel> modifierItems, int totalRecords)> GetModifierItemAsync(long modifierGroupId, int pageNo, int pageSize, string search)
    {
        IQueryable<ModifierItemViewModel> query = _context.Modifieritems
                                 .Include(m => m.Unit)
                                 .Where(m => m.ModifierGroupId == modifierGroupId && !m.Isdeleted)
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

    public async Task<ModifierGroupViewModel> GetModifierItemByIdAsync(long modifierId)
    {
        ModifierGroupViewModel? modifiergroup = await _context.Modifiergroups.Where(m => m.Id == modifierId).
                                    Include(m => m.Modifieritems).
                                    Select( m => new ModifierGroupViewModel {
                                        ModifierId = m.Id,
                                        Name = m.Name,
                                        Description = m.Description,
                                        ModifierItemList = m.Modifieritems.Select( i => new ModifierItemViewModel{
                                            ModifierItemId = i.Id,
                                            Name = i.Name,
                                            Rate = i.Rate,
                                            Quantity = i.Quantity,
                                        }).ToList()
                                    }).FirstOrDefaultAsync();

        return modifiergroup;
    }

    public async Task<List<ItemModifierGroupListViewModel>> GetModifierItemByItemId(long itemId)
    {
       List<ItemModifierGroupListViewModel> model =await _context.Itemmodifiergroups.Include(i => i.ModifierGroup.Modifieritems).
       Where(i => i.Itemid == itemId).Select(i => new ItemModifierGroupListViewModel{
        ItemId = i.Itemid,
        ModifierGroupId = i.ModifierGroupId,
        Name = i.ModifierGroup.Name,
        MinAllowed = i.MinAllowed,
        MaxAllowed = i.MaxAllowed,
        ModifierItemList = i.ModifierGroup.Modifieritems.Select( i => new ModifierItemViewModel{
            ModifierItemId = i.Id,
            Name = i.Name,
            Rate = i.Rate,
            Quantity = i.Quantity
        }).ToList()
       }).ToListAsync();


       return model;
    }
}
