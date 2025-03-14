using System.Text.Json.Nodes;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLogicLayer.Interfaces;

public interface IUserDetailService
{
    public JsonResult UserDetails(string token);

    public string UserName(string token);
    public string Email(string token);
    public Task<string> UsernameByEmail(string email);

    /*-------------------------------------------------------------------------------------------------------------Get User Details Method Implementation
    -----------------------------------------------------------------------------------------------------------------------------------------*/
    public Task<long> GetUserIdByUserNameAsync(string userName);

    public Task<ProfileDataViewModel> GetProfileData(string email);

    public Task<UserViewModel> GetUserDetails(int pageNo, int pageSize, string search,string columnName, string sortOrder);

    /*-------------------------------------------------------------------------------------------------------------Update Profile Data Method Implementation
    -----------------------------------------------------------------------------------------------------------------------------------------*/
    public Task<(string message, bool result)> UpdateUserProfileData(ProfileDataViewModel model, string email);

}
