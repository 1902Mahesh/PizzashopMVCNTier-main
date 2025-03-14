using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;

namespace DataLogicLayer.Interfaces;

public interface IModifiersRepository
{
    #region Modifier Group CRUD
    public IEnumerable<Modifiergroup> GetAllModifierGroup();
    public Task<AddEditModifierViewModel> GetModifierByIdAsync(long modifierId);
    public Task<string> AddModifierAsync(ModifierGroupViewModel model, long userId);
    public Task<string> EditModifierAsync(ModifierGroupViewModel model, long userId);
    public Task<bool> DeleteModifierGroupAsync(long modifierGroupId, long userId);
    #endregion

    #region Add Item Modifier Group CRUD
    public Task<ModifierGroupViewModel> GetModifierItemByIdAsync(long modifierId);
    public Task<(IEnumerable<ModifierItemViewModel> modifierItems, int totalRecords)> GetAllModifierItemAsync(int pageNo, int pageSize, string search);
    #endregion
    
    #region Modifier Items CRUD
    public Task<ModifierGroupViewModel> GetModifierGroupByIdAsync(long modifierId);
    public Task<(IEnumerable<ModifierItemViewModel> modifierItems, int totalRecords)> GetModifierItemAsync(long modifierGroupId, int pageNo, int pageSize, string search);
    public Task<List<ItemModifierGroupListViewModel>> GetModifierItemByItemId(long itemId);
    public Task<string> AddModifierItemAsync(AddEditModifierViewModel model, long userId);
    public Task<string> EditModifierItemAsync(AddEditModifierViewModel model, long userId);
    public Task<bool> DeleteModifierItemAsync(long modifierId, long userId);
    #endregion
}
