using System.Text.Json.Nodes;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLogicLayer.Interfaces;

public interface IUserDetailService
{
    public JsonResult UserDetails(string token);

    public string UserName(string token);
    public string Email(string token);

    /*-------------------------------------------------------------------------------------------------------------Get User Details Method Implementation
    -----------------------------------------------------------------------------------------------------------------------------------------*/
    public Task<long> GetUserIdByUserNameAsync(string userName);

    public Task<ProfileDataViewModel> GetProfileData(string email);

    public Task<UserViewModel> GetUserDetails(int pageNo, int pageSize, string search);

    /*-------------------------------------------------------------------------------------------------------------Update Profile Data Method Implementation
    -----------------------------------------------------------------------------------------------------------------------------------------*/
    public Task<bool> UpdateUserProfileData(ProfileDataViewModel model, string email);

}
