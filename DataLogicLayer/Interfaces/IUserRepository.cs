using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;

namespace DataLogicLayer.Interfaces;

public interface IUserRepository
{
    /*---------------------------------------------------------------------------Get User Method Definations
    -------------------------------------------------------------------------------------------------------*/
    public Task<User> GetUserByEmail(string email);

    public Task<User> GetUserByUserName(string userName);

    public Task<User> GetUserById(long id);


    /*---------------------------------------------------------------------------Add User Method Definations
    -------------------------------------------------------------------------------------------------------*/
    public Task<(User user, string message)> AddUserAsync(AddUserViewModel model, long adminId);


    /*---------------------------------------------------------------------------Update/Edit User Method Definations
    -------------------------------------------------------------------------------------------------------*/
    public Task<bool> UpdateUserPassword(User user, string password);
    public Task<(string message, bool result)> UpdateUserProfileData(User user, ProfileDataViewModel model);
    public Task<(string message, bool result)> EditUserAsync(AddUserViewModel model, User user, long adminId);

    /*---------------------------------------------------------------------------Delete/Soft Delete User Method Definations
    -------------------------------------------------------------------------------------------------------*/

    public Task<bool> SoftDeleteUserAsync(User user, long adminId);


}
