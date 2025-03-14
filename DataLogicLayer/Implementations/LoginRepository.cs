using System.Threading.Tasks;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLogicLayer.Implementations;

public class LoginRepository : ILoginRepository
{
    private readonly PizzaShopDbContext _context;


    public LoginRepository(PizzaShopDbContext context)
    {
        _context = context;

    }

    #region RESET password Token CRUD
    public async Task<string> GetEmailFromToken(string token)
    {
        ResetPasswordToken? resetToken = await _context.ResetPasswordTokens.Where(r => r.Token == token && !r.IsUsed).FirstOrDefaultAsync();
        return resetToken.Email;
    }

    public async Task<bool> SaveToken(User user, string token)
    {
        ResetPasswordToken resetToken = new ResetPasswordToken{
            Email = user.Email,
            Token = token
        };

        try
        {
            _context.ResetPasswordTokens.Add(resetToken);
            await _context.SaveChangesAsync();
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error in Reset Password", ex.Message);
            return false;
        }
    }

    public async Task<bool> SetResetToken(string token)
    {
        ResetPasswordToken? resetPasswordToken =await _context.ResetPasswordTokens.Where(r => r.Token == token && !r.IsUsed).FirstOrDefaultAsync();
        if(resetPasswordToken == null) return false;

        resetPasswordToken.IsUsed = true;
        _context.ResetPasswordTokens.Update(resetPasswordToken);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string> ValidateToken(string token)
    {
        ResetPasswordToken? resetPasswordToken = await _context.ResetPasswordTokens.Where(r => r.Token == token && !r.IsUsed).FirstOrDefaultAsync();
        if(resetPasswordToken == null) return "Link Expired";

        var difference = resetPasswordToken.Expirytime.Subtract(DateTime.Now).Ticks;
        if(difference > 0)
        {
            return "true";
        }
        return "Link Expired";
    }
    #endregion
}
