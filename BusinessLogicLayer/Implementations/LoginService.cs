using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BusinessLogicLayer.Implementations;

public class LoginService : ILoginService
{
    private readonly JwtService _jwtService;
    private readonly EncryptionService _encryptSercive;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
     private readonly IHttpContextAccessor _httpContextAccessor;


    public LoginService(JwtService jwtService, EncryptionService encryptService, IUserRepository userRepository, IRoleRepository roleRepository,
    IHttpContextAccessor httpContextAccessor)
    {
        _jwtService = jwtService;
        _encryptSercive = encryptService;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    /*-------------------------------------------------------------------------------------------------------------Login User Service Implementation
    -----------------------------------------------------------------------------------------------------------------------------------------*/

    #region Login With Refresh Token
    public async Task<bool> LoginRefresh(){
        HttpContext? context = _httpContextAccessor.HttpContext;

        string? token = context.Session.GetString("SuperSecretAuthToken");

        // If session is expired but "Remember Me" cookie exists, refresh the JWT token
        if(string.IsNullOrEmpty(token) && context.Request.Cookies["UserEmail"] != null){
            string? userEmail = context.Request.Cookies["UserEmail"];

            User user = await _userRepository.GetUserByEmail(userEmail);
            if(user == null){
                return false;
            }

            //Fetch the role 
            Role roleObj = await _roleRepository.GetRoleById(user.Roleid);
            // Generate a new JWT token
            string newToken = await _jwtService.GenerateJwtToken(userEmail, roleObj.Rolename, user.Username, user.Roleid);

            // Update the JWT token in the session
            if(newToken != null)
                context.Session.SetString("SuperSecretAuthToken", newToken);
            
            // Store the UserName and user Image Url in sessin
            _httpContextAccessor?.HttpContext?.Session.SetString("UserName", user.Username);
            _httpContextAccessor?.HttpContext?.Session.SetString("ProfileImage", user.Imgurl ?? "/images/default-profile.png");
            return true;
        }
        return false;
    }
    #endregion


    #region  Login User Service
    public async Task<bool> LoginUser(string email, string password, bool rememberMe)
    {
        //Get the user from ther user repository
        User user = await _userRepository.GetUserByEmail(email);
        if(user == null){
            return false;
        }
        

        //Fetch the role 
        Role roleObj = await _roleRepository.GetRoleById(user.Roleid);
        // Check for the Hashed Password

        if(user.Password == _encryptSercive.EncryptPassword(password)){

            // generate the Jwt token
            string token = await _jwtService.GenerateJwtToken(email, roleObj.Rolename, user.Username, user.Roleid);
            if(token != null){
                // Store JWT Token in Session
                _httpContextAccessor?.HttpContext?.Session.SetString("SuperSecretAuthToken", token);
                if (rememberMe)
                {
                    _httpContextAccessor?.HttpContext?.Response.Cookies.Append("UserEmail", email, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(7),
                        HttpOnly = false, // Allow access for autofill
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
                }

                // Store the UserName and user Image Url in sessin
                _httpContextAccessor?.HttpContext?.Session.SetString("UserName", user.Username);
                _httpContextAccessor?.HttpContext?.Session.SetString("ProfileImage", user.Imgurl ?? "/images/default-profile.png");
                return true;
            }
            return true;
        }
        return false;
    }
    #endregion

    #region Logout User
    public void Logout(){
        HttpContext? context = _httpContextAccessor.HttpContext;
        if (context!= null)
        {
            context.Session.Remove("SuperSecretAuthToken");
            context.Session.Remove("UserName");
            context.Session.Remove("ProfileImage");
            // Delete User Email Cookie
            if (context.Request.Cookies["UserEmail"] != null)
            {
                context.Response.Cookies.Delete("UserEmail");
            }
            context.Session.Clear();
        }
    }

    #endregion
}
