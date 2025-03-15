using System.ComponentModel;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DataLogicLayer.Implementations;

public class UserRepository : IUserRepository
{
    private readonly PizzaShopDbContext _context;


    public UserRepository(PizzaShopDbContext context)
    {
        _context = context;

    }
    #region Get User Implementations
    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.Where(u => u.Email == email && !u.Isdeleted).FirstOrDefaultAsync();
    }

    public async Task<User> GetUserById(long id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.Isdeleted);
    }


    public async Task<User> GetUserByUserName(string userName)
    {
        return await _context.Users.Where(u => u.Username == userName && !u.Isdeleted).FirstOrDefaultAsync();
    }
    #endregion

    #region Add User Implementation
    public async Task<(User? user, string? message)> AddUserAsync(AddUserViewModel model, long adminId)
    {
        try
        {
            User? existingUser = await _context.Users.Where(u => u.Email == model.Email).FirstOrDefaultAsync();
            User? existingUserName = await _context.Users.Where(u => u.Email == model.Email).FirstOrDefaultAsync();
            if (existingUser != null && existingUser.Isdeleted == false)
            {
                string message = "Email already exist!";
                return (null, message);
            }
            if (existingUserName != null && existingUserName.Isdeleted == false)
            {
                string message = "UserName already exist!";
                return (null, message);
            }

            if (existingUser != null && existingUser.Isdeleted == true)
            {
                existingUser.Email = string.Concat(existingUser.Email, DateTime.Now);
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
            }
            User user = new User
            {
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                Username = model.UserName,
                Password = model.Password,
                Roleid = model.RoleId,
                Countryid = model.CountryId,
                Stateid = model.StateId,
                Cityid = model.CityId,
                Address = model.Address,
                Zipcode = model.Zipcode,
                Phone = model.Phone,
                CreatedBy = adminId,
            };

            // Handle Image Upload
            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = $"{Guid.NewGuid()}_{model.ProfileImage.FileName}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(fileStream);
                }

                user.Imgurl = $"/uploads/{fileName}"; // Store relative path in DB
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return (user, "");
        }
        catch(DbUpdateException ex)
        {
            if(ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                return (null,"UserName already Exist!");
            }
            return(null, "Failed To Add User");
        }
    }
    #endregion

    #region  Update User Implementation
    public async Task<bool> UpdateUserPassword(User user, string password)
    {
        try
        {
            user.Password = password;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = user.Id;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("error message from Reset Password: " + ex.Message);
            return false;
        }
    }

    public async Task<(string message, bool result)> UpdateUserProfileData(User user, ProfileDataViewModel model)
    {
        try
        {
            User? existingUser = await _context.Users.Where(u => u.Username == model.userName && u.Id != user.Id).FirstOrDefaultAsync();
            if(existingUser != null && existingUser.Isdeleted == false)
            {
                return ("Username Already Exist!", false);
            }
            user.Firstname = model.firstName;
            user.Lastname = model.lastName;
            user.Username = model.userName;
            user.Phone = model.phoneNo;
            user.Zipcode = model.zipcode;
            user.Address = model.address;
            user.Countryid = model.CountryId;
            user.Stateid = model.StateId;
            user.Cityid = model.CityId;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = user.Id;


            // Handle Image Upload
            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = $"{Guid.NewGuid()}_{model.ProfileImage.FileName}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(fileStream);
                }

                user.Imgurl = $"/uploads/{fileName}"; // Store relative path in DB
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return ("Success", true);
        }
        catch (Exception ex)
        {
            if(ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                return ("UserName already Exist!", false);
            }
            return("Failed To Add User", false);
        }
    }

    public async Task<(string message, bool result)> EditUserAsync(AddUserViewModel model, User user, long adminId)
    {
        try
        {
            User? existingUser = await _context.Users.Where(u => u.Username == model.UserName && model.UserId != u.Id).FirstOrDefaultAsync();
            if(existingUser != null && existingUser.Isdeleted == false)
            {
                return("Username already Exist", false);
            }


            user.Firstname = model.FirstName;
            user.Lastname = model.LastName;
            user.Username = model.UserName;
            user.Email = model.Email;
            user.Roleid = model.RoleId;
            user.Status = model.Status;
            user.Countryid = model.CountryId;
            user.Stateid = model.StateId;
            user.Cityid = model.CityId;
            user.Phone = model.Phone;
            user.Address = model.Address;
            user.Zipcode = model.Zipcode;
            user.UpdatedBy = adminId; // Update the user who updated the record
            user.UpdatedAt = DateTime.Now; // Update the date when the record was updated


            // Handle Image Upload
            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = $"{Guid.NewGuid()}_{model.ProfileImage.FileName}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(fileStream);
                }

                user.Imgurl = $"/uploads/{fileName}"; // Store relative path in DB
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return ("", true);
        }
        catch (DbUpdateException ex)
        {
            if(ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                return ("UserName already Exist!", false);
            }
            return("Failed To Edit User", false);
        }
    }
    #endregion

    #region  Delete User Implementation
    public async Task<bool> SoftDeleteUserAsync(User user, long adminId)
    {
        user.Status = false;
        user.Isdeleted = true;
        user.UpdatedBy = adminId;
        user.UpdatedAt = DateTime.Now;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }
    #endregion

}
