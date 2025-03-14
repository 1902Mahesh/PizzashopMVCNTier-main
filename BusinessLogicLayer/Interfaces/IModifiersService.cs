using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Interfaces;

public interface IModifiersService
{
    public IEnumerable<ModifierGroupViewModel> GetAllModifierGroup();
    public Task<ModifierGroupViewModel> GetModifierItemById(long modifierId);
    public Task<List<ItemModifierGroupListViewModel>> GetAllModifierItemById(long itemId);


    public Task<ModifierItemListViewModel> GetModfierItems(long modifierGroupId, int pageNo, int pageSize, string search);
    public Task<ModifierItemListViewModel> GetAllModfierItems(int pageNo, int pageSize, string search);

    public Task<string> AddModifier(ModifierGroupViewModel model, long userId);
    public Task<string> EditModifier(ModifierGroupViewModel model, long userId);
    public Task<ModifierGroupViewModel> GetModifierGroupById(long modifierId);

    public Task<bool> DeleteModifier(long modifierGroupId, string userName);

    public Task<string> AddModifierItem(AddEditModifierViewModel model, long userId);
    public Task<string> EditModifierItem(AddEditModifierViewModel model, long userId);
    public Task<AddEditModifierViewModel> GetModifierById(long modifierId);
    public Task<bool> DeleteModifierItem(long modifierId, string userName);

}
