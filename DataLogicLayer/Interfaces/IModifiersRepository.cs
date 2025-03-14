using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;

namespace DataLogicLayer.Interfaces;

public interface IModifiersRepository
{
    #region Modifier Group CRUD
    public IEnumerable<Modifiergroup> GetAllModifierGroup();
    public Task<ModifierGroupViewModel> GetModifierGroupByIdAsync(long modifierGroupId);
    public Task<ModifierGroupViewModel> GetModifierItemByIdAsync(long modifierGroupId); // For Existing Modifiers In Edit Modifier Item
    public Task<(IEnumerable<ModifierItemViewModel> modifierItems, int totalRecords)> GetAllModifierItemAsync(int pageNo, int pageSize, string search); // Item List for Existing Modifier In Add Modifier Item
    public Task<string> AddModifierGroupAsync(ModifierGroupViewModel model, long userId);
    public Task<string> EditModifierGroupAsync(ModifierGroupViewModel model, long userId);
    public Task<bool> DeleteModifierGroupAsync(long modifierGroupId, long userId);
    #endregion
    
    #region Modifier Items CRUD
    public Task<(IEnumerable<ModifierItemViewModel> modifierItems, int totalRecords)> GetModifierItemAsync(long modifierGroupId, int pageNo, int pageSize, string search);
    public Task<AddEditModifierViewModel> GetModifierByIdAsync(long modifierId); //For Edit Modifier Item
    public Task<string> AddModifierItemAsync(AddEditModifierViewModel model, long userId);
    public Task<string> EditModifierItemAsync(AddEditModifierViewModel model, long userId);
    public Task<bool> DeleteModifierItemAsync(long modifierId, long userId);
    #endregion

    #region Selected Modifier Items For Edit Item Modal
    public Task<List<ItemModifierGroupListViewModel>> GetModifierItemByItemId(long itemId);
    #endregion
}
