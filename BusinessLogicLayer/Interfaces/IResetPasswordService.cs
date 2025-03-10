using System.Runtime.CompilerServices;

namespace BusinessLogicLayer.Interfaces;

public interface IResetPasswordService
{
    public Task<bool> ResetPassword(string password, string newPassword, string token);

    public Task<string> validateToken(string token);
}
