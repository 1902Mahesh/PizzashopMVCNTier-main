using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Implementations;

public class ChangePasswordService : IChangePasswordService
{
    private readonly IUserRepository _userRepository;
    private readonly IResetPasswordService _resetPasswordService;
    private readonly EncryptionService _encryptionService;

    public ChangePasswordService(IUserRepository userRepository, IResetPasswordService resetPasswordService, EncryptionService encryptionService)
    {
        _userRepository = userRepository;
        _resetPasswordService = resetPasswordService;
        _encryptionService = encryptionService;
    }


    /*-------------------------------------------------------------------------------------------------------------Change User Password Service Implementation
    ------------------------------------------------------------------------------------------------------------------------------------------------------*/

    #region Change Password Method
    public async Task<bool> ChangePassword(string currentPassword, string newPassword, string confirmNewPassword, string email)
    {
        if (!string.IsNullOrEmpty(currentPassword) && !string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(confirmNewPassword) && newPassword.Equals(confirmNewPassword))
        {
            User user = await _userRepository.GetUserByEmail(email);
            if(user == null){
                return false;
            }
            // Check if current password Matches with Database
            if(user.Password != _encryptionService.EncryptPassword(currentPassword)){
                return false;
            }
            // Encrypted Password Saving
            string HashedPassword = _encryptionService.EncryptPassword(newPassword);
            return await _userRepository.UpdateUserPassword(user, HashedPassword);
        }
        return false;
    }
    #endregion

}
