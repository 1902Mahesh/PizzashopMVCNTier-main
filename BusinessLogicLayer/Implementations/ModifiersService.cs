using BusinessLogicLayer.Common;
using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Implementations;

public class ModifiersService : IModifiersService
{
    private readonly IModifiersRepository _modifiersRepository;


    public ModifiersService(IModifiersRepository modifiersRepository)
    {
        _modifiersRepository = modifiersRepository;

    }
    public IEnumerable<ModifierGroupViewModel> GetAllModifierGroup()
    {
        IEnumerable<Modifiergroup> modifiergroups = _modifiersRepository.GetAllModifierGroup();
        return modifiergroups.Select(m => new ModifierGroupViewModel
        {
            ModifierId = m.Id,
            Name = m.Name,
            Description = m.Description
        });
    }

    public Task<List<ItemModifierGroupListViewModel>> GetAllModifierItemById(long itemId)
    {
        return _modifiersRepository.GetModifierItemByItemId(itemId);
    }


    public async Task<ModifierItemListViewModel> GetModfierItems(long modifierGroupId = 1, int pageNo = 1, int pageSize = 3, string search = "")
    {
        ModifierItemListViewModel model = new() { Page = new() };
        var modifierItemData = await _modifiersRepository.GetModifierItemAsync(modifierGroupId, pageNo, pageSize, search);;

        model.ModifierItemList = modifierItemData.modifierItems;
        model.Page.SetPagination(modifierItemData.totalRecords, pageSize, pageNo);
        return model;
    }

    public async Task<ModifierGroupViewModel> GetModifierItemById(long modifierId)
    {
        return await _modifiersRepository.GetModifierItemByIdAsync(modifierId);
    }

}
