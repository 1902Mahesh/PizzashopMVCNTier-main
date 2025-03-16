using System.Diagnostics;
using System.Threading.Tasks;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Constants;
using BusinessLogicLayer.Interfaces;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PizzashopMVCNtier.Models;

namespace PizzashopMVCNtier.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ILoginService _loginService;
    private readonly IForgotPasswordService _forgotPasswordService;
    private readonly IResetPasswordService _resetPasswordService;

    public HomeController(ILogger<HomeController> logger, ILoginService loginService, IForgotPasswordService forgotPasswordService, IResetPasswordService resetPasswordService)
    {
        _logger = logger;
        _loginService = loginService;
        _forgotPasswordService = forgotPasswordService;
        _resetPasswordService = resetPasswordService;
    }

/*---------------------------------------------------------------------------Login------------------------------------------------------------------------------*/

    #region Login
    [HttpGet]
    public async Task<IActionResult> Login()
    {
        bool result = await _loginService.LoginRefresh();
        if (result)
        {
            return RedirectToAction("Index", "Dashboard");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        bool token = await _loginService.LoginUser(model.Email, model.Password, model.RememberMe);
        if (token)
        {
            // TempData["NotificationMessage"] = NotificationMessages.LoginSuccess;
            // TempData["NotificationType"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", "Dashboard");
        }
        //If Login Service return false
        TempData["NotificationMessage"] = NotificationMessages.InvalidCredentials;
        TempData["NotificationType"] = NotificationType.Error.ToString();
        return View(model);
    }
    #endregion

/*---------------------------------------------------------------------------Logout------------------------------------------------------------------------------*/

    #region Logout
    [Authorize]
    public IActionResult Logout()
    {
        _loginService.Logout();
        return RedirectToAction("Login", "Home");
    }
    #endregion

/*---------------------------------------------------------------------------Forgot Password------------------------------------------------------------------------------*/

    #region Forgot Password
    [HttpGet]
    public IActionResult ForgotPassword(string email)
    {
        ViewBag.UserEmail = string.IsNullOrEmpty(email) ? "" : email;
        return View();
    }


    [HttpPost]  
    public async Task<IActionResult> ForgotPassword(ForgotPassViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        string token = Guid.NewGuid().ToString();
        string? resetPasswordLink = Url.Action("ResetPassword", "Home", new {token }, Request.Scheme);

        bool result = await _forgotPasswordService.ForgotPassword(model.Email, resetPasswordLink, token);

        // If Email is send then Toast Message Display
        if (result)
        {
            TempData["NotificationMessage"] = NotificationMessages.EmailSentSuccessfully;
            TempData["NotificationType"] = NotificationType.Success.ToString();
            return RedirectToAction("Login", "Home");
        }
        TempData["NotificationMessage"] = NotificationMessages.EmailSendingFailed;
        TempData["NotificationType"] = NotificationType.Error.ToString();
        return View();
    }

    #endregion

/*---------------------------------------------------------------------------Reset Password------------------------------------------------------------------------------*/

    #region Reset Password

    [HttpGet]
    public async Task<IActionResult> ResetPassword(string token)
    {
        var result = await _resetPasswordService.validateToken(token);
        if(result != "true")
        {
            TempData["NotificationMessage"] = result;
            TempData["NotificationType"] = NotificationType.Error.ToString();
            return RedirectToAction("Login", "Home");
        }
        ViewData["Token"] = token;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPassViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        bool result = await _resetPasswordService.ResetPassword(model.Password, model.ConfirmPassword, model.Token);
        // If password Reset Successfully
        if (result)
        {
            TempData["NotificationMessage"] = NotificationMessages.PasswordChanged;
            TempData["NotificationType"] = NotificationType.Success.ToString();
            return RedirectToAction("Login", "Home");
        }
        TempData["NotificationMessage"] = NotificationMessages.PasswordChangeFailed;
        TempData["NotificationType"] = NotificationType.Error.ToString();
        return View(model);
    }
    #endregion

/*---------------------------------------------------------------------------Error Controll------------------------------------------------------------------------------*/

    #region Error Controll
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("Home/Error/{code}")]
    public IActionResult Error(int code)
    {
        if (code == 404)
        {
            return View("404"); // Custom 404 page
        }
        else if (code == 500)
        {
            return View("500"); // Custom 500 page
        }
        else if(code == 401 || code == 403)
        {
            ViewData["StatusCode"] = code;
            return View("Unauthorized");
        }
        return View("Error"); // General error page
    }
    #endregion
}
