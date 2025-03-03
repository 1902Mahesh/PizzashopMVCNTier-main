using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;

namespace BusinessLogicLayer.Implementations;

public class ResetPasswordService : IResetPasswordService
{
    private readonly IUserRepository _userRepository;
    private readonly EncryptionService _encryptionService;
    public ResetPasswordService(IUserRepository userRepository, EncryptionService encryptionService)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
    }

    /*-------------------------------------------------------------------------------------------------------------Reset Password Service Implementation
    -----------------------------------------------------------------------------------------------------------------------------------------*/

    #region  Reset Password
    public async Task<bool> ResetPassword(string password, string newPassword, string email)
    {
        if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(newPassword) && password.Equals(newPassword))
        {

            // Fetch user from the database
            User user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }
            // Encrypted Password Saving
            string HashedPassword = _encryptionService.EncryptPassword(password);
            return await _userRepository.UpdateUserPassword(user, HashedPassword);
        }
        return false;
    }
    #endregion
}
