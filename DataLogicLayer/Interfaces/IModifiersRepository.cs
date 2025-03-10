using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;

namespace DataLogicLayer.Interfaces;

public interface IModifiersRepository
{
    public IEnumerable<Modifiergroup> GetAllModifierGroup();

    public Task<ModifierGroupViewModel> GetModifierItemByIdAsync(long modifierId);
    public Task<List<ItemModifierGroupListViewModel>> GetModifierItemByItemId(long itemId);

    public Task<(IEnumerable<ModifierItemViewModel> modifierItems, int totalRecords)> GetModifierItemAsync(long modifierGroupId, int pageNo, int pageSize, string search);
}
