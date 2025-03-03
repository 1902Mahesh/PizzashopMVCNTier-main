using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Interfaces;

public interface ILoginService
{
    public Task<bool> LoginRefresh();
    public Task<bool> LoginUser(string email, string password, bool rememberMe);

    public void Logout();

}
