using System;
using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLogicLayer.Implementations;

public class ForgotPasswordService : IForgotPasswordService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly ILoginRepository _loginRepository;

    public ForgotPasswordService(IUserRepository userRepository, IEmailSender emailSender, ILoginRepository loginRepository)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _loginRepository = loginRepository;
    }

    /*-------------------------------------------------------------------------------------------------------------Forgot Password Service Implementation
    -----------------------------------------------------------------------------------------------------------------------------------------*/


    #region Forgot password Service
    public async Task<bool> ForgotPassword(string email, string? resetPasswordLink, string emailToken)
    {
        string subject = "Password Reset Link";
        string emailBody = $@"<div
        class=''
        style='background-color: #0066a7; display:flex; justify-content:center; align-items: center;'
        >
        <img src='https://images.vexels.com/media/users/3/128437/isolated/preview/2dd809b7c15968cb7cc577b2cb49c84f-pizza-food-restaurant-logo.png' alt='' width='50px' />
        <span style='color: #ffffff; font-weight: 550; font-size: 25px'
            >PIZZASHOP</span
        >
        </div>
        <div style='background-color: #f2f2f2'>
        <p>Pizza shop,</p>
        <p>Please click <a style='color:blue; text-decoration:underline;' href='{resetPasswordLink}' >here</a> for reset your Account Password.</p>
        <p>
            If you encounter any issues or have any questions, please do not
            hesitate to contact our support team.
        </p>
        <p>
            <span style='color: orange'>Important Note:</span>
            For security reasons, the link will expire in 24 hours, if you did not
            request a password reset, please ignore this email or contact our
            support team immediately.
        </p>
        </div>
            ";




        User user = await _userRepository.GetUserByEmail(email);
        if (user == null)
        {
            return false;
        }

        bool result = await _loginRepository.SaveToken(user, emailToken);

        if (result)
        {
            await _emailSender.SendEmailAsync(email, subject, emailBody);
            return true;
        }
        return false;
    }


    #endregion
}
