using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;

namespace DataLogicLayer.Interfaces;

public interface ITableSectionRepository
{
    #region Sections CRUD
    public Task<List<Section>> GetSectionsListAsync();
    public Task<Section> GetSectionByIdAsync(long sectionId);
    public Task<string> AddSectionAsync(SectionViewModel model, long userId);
    public Task<string> EditSectionAsync(SectionViewModel model, long userId);
    public Task<bool> DeleteSectionAsync(long sectionId, long userId);
    #endregion

    #region Tables CRUD
    public Task<(List<TableViewModel> tables, int totalRecords)> GetTableListAsync(long sectionId, int pageNo, int pageSize, string search);
    #endregion
}
