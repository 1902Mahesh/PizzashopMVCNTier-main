using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Interfaces;

public interface IChangePasswordService
{
    public Task<bool> ChangePassword(string currentPassword, string newPassword, string confirmNewPassword, string email);
}
