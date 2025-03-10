using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Interfaces;

public interface IModifiersService
{
    public IEnumerable<ModifierGroupViewModel> GetAllModifierGroup();
    public Task<ModifierGroupViewModel> GetModifierItemById(long modifierId);
    public Task<List<ItemModifierGroupListViewModel>> GetAllModifierItemById(long itemId);


    public Task<ModifierItemListViewModel> GetModfierItems(long modifierGroupId, int pageNo, int pageSize, string search);
}
