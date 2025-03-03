using System.Text.Json.Nodes;
using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLogicLayer.Implementations;

public class UserDetailService : IUserDetailService
{
    private readonly JwtService _jwtService;
    private readonly IGetUserRecordsRepository _userRecordsRepository;

    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ICountryDetailRepository _countryDetailRepository;
     private readonly IHttpContextAccessor _httpContextAccessor;



    public UserDetailService(JwtService jwtService, IGetUserRecordsRepository userRecordsRepository, 
                IUserRepository userRepository, IRoleRepository roleRepository, ICountryDetailRepository countryDetailRepository,
                IHttpContextAccessor httpContextAccessor)
    {
        _jwtService = jwtService;
        _userRecordsRepository = userRecordsRepository;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _countryDetailRepository = countryDetailRepository;
    }



    #region Loged In User Details Service
    public string Email(string token)
    {
        return _jwtService.GetClaimValue(token, "email");
    }

    public string UserName(string token)
    {
        return _jwtService.GetClaimValue(token, "userName");
    }

    public JsonResult UserDetails(string token)
    {
        string email = _jwtService.GetClaimValue(token, "email");
        string userName = _jwtService.GetClaimValue(token, "userName");
        return new JsonResult(new { Email = email, UserName = userName });
    }

    #endregion


    #region Profile Data Service
    public async Task<ProfileDataViewModel> GetProfileData(string email)
    {
        User user = await _userRepository.GetUserByEmail(email);
        if (user == null)
        {
            return new ProfileDataViewModel();
        }

        Role roleObj = await _roleRepository.GetRoleById(user.Roleid);

        return new ProfileDataViewModel()
        {
            firstName = user.Firstname,
            lastName = user.Lastname,
            userName = user.Username,
            Role = roleObj.Rolename,
            phoneNo = user.Phone,
            zipcode = user.Zipcode,
            address = user.Address,
            Imgurl = user.Imgurl,
            CountryId = user.Countryid,
            CityId = user.Cityid,
            StateId = user.Stateid,
            Countries = _countryDetailRepository.GetCountry(),
            States = _countryDetailRepository.GetState(user.Countryid),
            Cities = _countryDetailRepository.GetCity(user.Stateid)
        };
    }

    public async Task<UserViewModel> GetUserDetails(int pageNo, int pageSize, string search)
    {
        UserViewModel model = await _userRecordsRepository.GetAllUserRecordsAsync(pageNo, pageSize, search);
        return model;
    }

    public async Task<long> GetUserIdByUserNameAsync(string userName)
    {
        User user = await _userRepository.GetUserByUserName(userName);
        if(user != null){
            return user.Id;
        }
        return -1;
    }
    #endregion


    #region Update Profile Data Service
    public async Task<bool> UpdateUserProfileData(ProfileDataViewModel model, string email)
    {
        User user = await _userRepository.GetUserByEmail(email);
        if(user == null)
        {
            return false;
        }
        bool result = await _userRepository.UpdateUserProfileData(user, model);
        if(!result)
        {
            return false;
        }
        HttpContext? context = _httpContextAccessor.HttpContext;
        context.Session.Remove("UserName");
        context.Session.Remove("ProfileImage");

        context.Session.SetString("UserName", user.Username);
        context.Session.SetString("ProfileImage", user.Imgurl ?? "/uploads/Default_pfp.svg.png");
        return true;
    }

    #endregion
}
