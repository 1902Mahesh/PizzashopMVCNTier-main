using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Implementations;

public interface ITableSectionService
{
    public Task<IEnumerable<SectionViewModel>> GetSectionsList();
    public Task<SectionViewModel> GetSectionById(long sectionId);
    public Task<string> AddSection(SectionViewModel model, long userId);
    public Task<string> EditSection(SectionViewModel model, long userId);
    public Task<bool> DeleteSection(long id, string userName);
    public Task<TableListViewModel> GetTableList(long sectionId,int pageNo, int pageSize, string search);

}
