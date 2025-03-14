using BusinessLogicLayer.Common;
using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Implementations;

public class ModifiersService : IModifiersService
{
    private readonly IModifiersRepository _modifiersRepository;
    private readonly IUserRepository _userRepository;

    public ModifiersService(IModifiersRepository modifiersRepository, IUserRepository userRepository)
    {
        _modifiersRepository = modifiersRepository;
        _userRepository = userRepository;
    }

    /*---------------------------------------------------------------------------Modifier GROUP CRUD------------------------------------------------------------------------------*/

    #region Modifier Group CRUD
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
    public async Task<ModifierGroupViewModel> GetModifierGroupById(long modifierId)
    {
        return await _modifiersRepository.GetModifierGroupByIdAsync(modifierId);
    }

    public async Task<string> AddModifier(ModifierGroupViewModel model, long userId)
    {
        if (model == null)
        {
            return "Model is Empty";
        }
        return await _modifiersRepository.AddModifierAsync(model, userId);
    }

    public async Task<string> EditModifier(ModifierGroupViewModel model, long userId)
    {
        if (model == null)
        {
            return "Model is Empty";
        }
        return await _modifiersRepository.EditModifierAsync(model, userId);
    }

    

    public async Task<bool> DeleteModifier(long modifierGroupId, string userName)
    {
        if (modifierGroupId == 0 || modifierGroupId == null)
        {
            return false;
        }

        User user = await _userRepository.GetUserByUserName(userName);
        if (user == null)
        {
            return false;
        }

        return await _modifiersRepository.DeleteModifierGroupAsync(modifierGroupId, user.Id);
    }

    #endregion

    /*---------------------------------------------------------------------------Modifier Items CRUD------------------------------------------------------------------------------*/

    #region Modifier Items CRUD
    public Task<List<ItemModifierGroupListViewModel>> GetAllModifierItemById(long itemId)
    {
        return _modifiersRepository.GetModifierItemByItemId(itemId);
    }
    public async Task<ModifierGroupViewModel> GetModifierItemById(long modifierId)
    {
        return await _modifiersRepository.GetModifierItemByIdAsync(modifierId);
    }

    public async Task<ModifierItemListViewModel> GetModfierItems(long modifierGroupId = 1, int pageNo = 1, int pageSize = 3, string search = "")
    {
        ModifierItemListViewModel model = new() { Page = new() };
        var modifierItemData = await _modifiersRepository.GetModifierItemAsync(modifierGroupId, pageNo, pageSize, search); ;

        model.ModifierItemList = modifierItemData.modifierItems;
        model.Page.SetPagination(modifierItemData.totalRecords, pageSize, pageNo);
        return model;
    }

    public async Task<string> AddModifierItem(AddEditModifierViewModel model, long userId)
    {
        if (model == null)
        {
            return "Model is Empty";
        }
        return await _modifiersRepository.AddModifierItemAsync(model, userId);
    }
    public async Task<string> EditModifierItem(AddEditModifierViewModel model, long userId)
    {
        if (model == null)
        {
            return "Model is Empty";
        }
        return await _modifiersRepository.EditModifierItemAsync(model, userId);
    }

    public async Task<AddEditModifierViewModel> GetModifierById(long modifierId)
    {
        return await _modifiersRepository.GetModifierByIdAsync(modifierId);
    }

    public async Task<bool> DeleteModifierItem(long modifierId, string userName)
    {
        if (modifierId == 0 || modifierId == null)
        {
            return false;
        }

        User user = await _userRepository.GetUserByUserName(userName);
        if (user == null)
        {
            return false;
        }

        return await _modifiersRepository.DeleteModifierItemAsync(modifierId, user.Id);
    }

    #endregion
    
    /*---------------------------------------------------------------------------Pagination Modal Modifeir Item CRUD------------------------------------------------------------------------------*/

    #region Modifier Item For Modal Pagination
    public async Task<ModifierItemListViewModel> GetAllModfierItems(int pageNo = 1, int pageSize = 3, string search = "")
    {
        ModifierItemListViewModel model = new() { Page = new() };
        var modifierItemData = await _modifiersRepository.GetAllModifierItemAsync(pageNo, pageSize, search);
        model.ModifierItemList = modifierItemData.modifierItems;
        model.Page.SetPagination(modifierItemData.totalRecords, pageSize, pageNo);
        return model;
    }
    #endregion
}
