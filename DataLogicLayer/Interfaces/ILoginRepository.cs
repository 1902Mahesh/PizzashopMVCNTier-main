using DataLogicLayer.Models;

namespace DataLogicLayer.Interfaces;

public interface ILoginRepository
{
    public Task<bool> SaveToken(User user,string token);
    public Task<string> ValidateToken(string token);
    public Task<bool> SetResetToken(string token);
    public Task<string> GetEmailFromToken(string token);

}
