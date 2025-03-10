using DataLogicLayer.ViewModels;

namespace DataLogicLayer.Interfaces;

public interface IGetUserRecordsRepository
{
    public Task<(List<UserListViewModel> users, int totalRecords)> GetAllUserRecordsAsync(int pageNo = 1, int pageSize = 3, string search = "",string columnName="", string sortOrder="");
}
