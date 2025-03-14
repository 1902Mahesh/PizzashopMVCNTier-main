namespace BusinessLogicLayer.Interfaces;

public interface IForgotPasswordService
{
    public Task<bool> ForgotPassword(string email, string? resetPasswordLink, string emailToken);


}
