using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BusinessLogicLayer.Implementations;

public class UserService : IUserService
{
    private readonly EncryptionService _encryptionService;
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly ICountryDetailRepository _countryDetailRepository;
    private readonly IRoleRepository _roleRepository;


    public UserService(EncryptionService encryptionService, IUserRepository userRepository, IEmailSender emailSender, 
        ICountryDetailRepository countryDetailRepository, IRoleRepository roleRepository)
    {
        _encryptionService = encryptionService;
        _userRepository = userRepository;
        _emailSender = emailSender;
        _countryDetailRepository = countryDetailRepository;
        _roleRepository = roleRepository;
    }


    #region Get User By Id Service
    public async Task<AddUserViewModel> GetUserByIdAsync(long id)
    {
        User user = await _userRepository.GetUserById(id);
        if(user == null){
            return new AddUserViewModel();
        }

        return new AddUserViewModel()
        {
            UserId = user.Id,
            FirstName = user.Firstname,
            LastName = user.Lastname,
            UserName = user.Username,
            RoleId = user.Roleid,
            Email = user.Email,
            ImgUrl = user.Imgurl,
            Status = user.Status,
            CountryId = user.Countryid,
            StateId = user.Stateid,
            CityId = user.Cityid,
            Phone = user.Phone,
            Address = user.Address,
            Zipcode = user.Zipcode,
            Roles = await _roleRepository.GetRoles(),
            Countries =_countryDetailRepository.GetCountry(),
            States = _countryDetailRepository.GetState(user.Countryid),
            Cities = _countryDetailRepository.GetCity(user.Stateid)
        };
    }

    #endregion

    #region Add User Service
    public async Task<(string message, bool result)> AddUserAsync(AddUserViewModel model, string userName)
    {
        string _password = model.Password;
        model.Password = _encryptionService.EncryptPassword(model.Password);

        User adminUser  = await _userRepository.GetUserByUserName(userName);

        (User user, string message) = await _userRepository.AddUserAsync(model, adminUser.Id);
        
        if(user != null){
            // Prepare Email Content
            string subject = "Welcome to Pizza Shop!";
            string body = $@"
                <h2>Welcome, {user.Firstname}!</h2>
                <p>Your account has been created successfully.</p>
                <p>Temporary Password: <strong>{_password}</strong></p>
                <br>
                <p>Best regards,<br>Pizza Shop Team</p>";

            // Send Email
            await _emailSender.SendEmailAsync(user.Email, subject, body);
            return (message, true);
        }
        return (message, false);
    }

    #endregion

    #region Update User Service
    public async Task<(string message, bool result)> UpdateUserAsync(AddUserViewModel model, string userName)
    {
        User admin = await _userRepository.GetUserByUserName(userName);
        User user = await _userRepository.GetUserById((long)model.UserId);
        if(user == null){
            return ("user doesn't Exist", false);
        }
        return await _userRepository.EditUserAsync(model, user, admin.Id);
    }
    #endregion

    #region Delete User Service
    public async Task<bool> DeleteUserAsync(long id, string adminName)
    {
        User admin = await _userRepository.GetUserByUserName(adminName);
        User user = await _userRepository.GetUserById(id);
        if(user == null){
            return false;
        }
        return await _userRepository.SoftDeleteUserAsync(user, admin.Id);
    }
    #endregion
}
