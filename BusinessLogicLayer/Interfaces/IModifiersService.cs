using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Interfaces;

public interface IModifiersService
{
    #region Modifier Group CRUD
    public IEnumerable<ModifierGroupViewModel> GetAllModifierGroup();
    public Task<ModifierGroupViewModel> GetModifierGroupById(long modifierGroupId);
    public Task<ModifierGroupViewModel> GetModifierItemById(long modifierGroupId); // For Existing Modifiers In Edit Modifier Item
    public Task<ModifierItemListViewModel> GetAllModfierItems(int pageNo, int pageSize, string search); // Item List for Existing Modifier In Add Modifier Item
    public Task<string> AddModifierGroup(ModifierGroupViewModel model, long userId);
    public Task<string> EditModifierGroup(ModifierGroupViewModel model, long userId);
    public Task<bool> DeleteModifierGroup(long modifierGroupId, string userName);
    #endregion

    #region Modifier Items CRUD
    public Task<ModifierItemListViewModel> GetModfierItems(long modifierGroupId, int pageNo, int pageSize, string search);  //Item List for Modifier Page
    public Task<AddEditModifierViewModel> GetModifierById(long modifierId); //For Edit Modifier Item
    public Task<string> AddModifierItem(AddEditModifierViewModel model, long userId);
    public Task<string> EditModifierItem(AddEditModifierViewModel model, long userId);
    public Task<bool> DeleteModifierItem(long modifierId, string userName);
    #endregion

    #region Selected Modifier Items For Edit Item Modal
    public Task<List<ItemModifierGroupListViewModel>> GetAllModifierItemById(long itemId);
    #endregion
}
