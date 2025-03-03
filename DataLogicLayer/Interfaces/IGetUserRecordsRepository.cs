using DataLogicLayer.ViewModels;

namespace DataLogicLayer.Interfaces;

public interface IGetUserRecordsRepository
{
    public Task<UserViewModel> GetAllUserRecordsAsync(int pageNo = 1, int pageSize = 3, string search = "");
}
