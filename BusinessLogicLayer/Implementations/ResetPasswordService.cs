using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;

namespace BusinessLogicLayer.Implementations;

public class ResetPasswordService : IResetPasswordService
{
    private readonly IUserRepository _userRepository;
    private readonly EncryptionService _encryptionService;
    private readonly ILoginRepository _loginRepository;


    public ResetPasswordService(IUserRepository userRepository, EncryptionService encryptionService, ILoginRepository loginRepository)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
        _loginRepository = loginRepository;

    }

    /*-------------------------------------------------------------------------------------------------------------Reset Password Service Implementation
    -----------------------------------------------------------------------------------------------------------------------------------------*/

    #region  Reset Password
    public async Task<bool> ResetPassword(string password, string newPassword, string token)
    {
        string validateToken = await _loginRepository.ValidateToken(token);
        if(validateToken != "true")
        {
            return false;
        }

        string email = await _loginRepository.GetEmailFromToken(token);

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
            if(await _userRepository.UpdateUserPassword(user, HashedPassword))
            {
                return await _loginRepository.SetResetToken(token);
            }
        }
        return false;
    }

    public Task<string> validateToken(string token)
    {
        return _loginRepository.ValidateToken(token);
    }

    #endregion
}
