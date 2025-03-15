using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Interfaces;

public interface IUserService
{
    public Task<AddUserViewModel> GetUserByIdAsync(long id);
    
    public Task<(string message, bool result)> AddUserAsync(AddUserViewModel model, string userName);


    public Task<(string message, bool result)> UpdateUserAsync(AddUserViewModel model, string userName);

    public Task<bool> DeleteUserAsync(long id, string adminName);

}