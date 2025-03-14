using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DataLogicLayer.Implementations;

public class UserRecordsRepository : IGetUserRecordsRepository
{
    private readonly PizzaShopDbContext _context;
    public UserRecordsRepository(PizzaShopDbContext context)
    {
        _context = context;
    }


    /*-----------------------------------------------------------------------------------------------Get User Records based on Search, Page no Method Implementation
    --------------------------------------------------------------------------------------------------------------------------------------------------------------*/

    #region  Get User Records for Pagination
    public async Task<(List<UserListViewModel> users, int totalRecords)> GetAllUserRecordsAsync(int pageNo, int pageSize, string search, string columnName, string sortOrder)
    {

        IQueryable<UserListViewModel> query = _context.Users
                                            .Include(u => u.Role)
                                            .Where(u => !u.Isdeleted)
                                            .Select(u => new UserListViewModel
                                            {
                                                UserId = u.Id,
                                                FirstName = u.Firstname,
                                                LastName = u.Lastname,
                                                Email = u.Email,
                                                Phone = u.Phone,
                                                Status = u.Status,  
                                                RoleName = u.Role.Rolename,
                                                ImgUrl = u.Imgurl
                                            });

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(u => u.FirstName.ToLower().Contains(search) ||
                                u.LastName.ToLower().Contains(search) ||
                                u.RoleName.ToLower().Contains(search) ||
                                u.Email.ToLower().Contains(search));
        }

        switch (columnName.ToLower())
        {
            case "name":
                query = (sortOrder == "asc") ? query.OrderBy(u => u.FirstName).ThenBy(u => u.LastName) : query.OrderByDescending(u => u.FirstName).ThenByDescending(u => u.LastName);
                break;
            case "role":
                query = (sortOrder == "asc") ? query.OrderBy(u => u.RoleName) : query.OrderByDescending(u => u.RoleName);
                break;
            default:
                query = query.OrderBy(u => u.UserId);
                break;
        }

        int totalRecords = await query.CountAsync();
        List<UserListViewModel> users = await query
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

        return (users, totalRecords);
    }
    #endregion

}
