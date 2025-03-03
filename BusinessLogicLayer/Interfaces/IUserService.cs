using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Interfaces;

public interface IUserService
{
    public Task<EditUserViewModel> GetUserByIdAsync(long id);
    
    public Task<bool> AddUserAsync(AddUserViewModel model, string userName);


    public Task<bool> UpdateUserAsync(EditUserViewModel model, string userName);

    public Task<bool> DeleteUserAsync(long id, string adminName);

}