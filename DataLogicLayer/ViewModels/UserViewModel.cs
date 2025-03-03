using System;

namespace DataLogicLayer.ViewModels;

public class UserViewModel
{
    public List<UserListViewModel> UserList { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
}