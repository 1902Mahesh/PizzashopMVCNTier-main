using System.Threading.Tasks;
using BusinessLogicLayer.Common;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Implementations;

public class TableSectionService : ITableSectionService
{
    private readonly ITableSectionRepository _tableSectionRepository;
    private readonly IUserRepository _userRepository;

    public TableSectionService(ITableSectionRepository tableSectionRepository, IUserRepository userRepository)
    {
        _tableSectionRepository = tableSectionRepository;
        _userRepository = userRepository;
    }

    #region Sections CRUD
    public async Task<IEnumerable<SectionViewModel>> GetSectionsList()
    {
        IEnumerable<Section> sections = await _tableSectionRepository.GetSectionsListAsync();
        return sections.Select(s => new SectionViewModel{
            SectionID = s.SectionId,
            Name = s.Name,
            Description = s.Description
        });
    }

    public async Task<SectionViewModel> GetSectionById(long sectionId)
    {
        Section section = await _tableSectionRepository.GetSectionByIdAsync(sectionId);
        SectionViewModel model = new SectionViewModel{
            SectionID = section.SectionId,
            Name = section.Name,
            Description = section.Description
        };
        return model;
    }

    public async Task<string> AddSection(SectionViewModel model, long userId)
    {
        if(model == null)
        {
            return "Model is Empty";
        }
        return await _tableSectionRepository.AddSectionAsync(model, userId);
    }

    public async Task<string> EditSection(SectionViewModel model, long userId)
    {
        if(model == null)
        {
            return "Model is Empty";
        }
        return await _tableSectionRepository.EditSectionAsync(model, userId);
    }

    #endregion

    public async Task<TableListViewModel> GetTableList(long sectionId,int pageNo, int pageSize, string search)
    {
        TableListViewModel model = new() {Page = new ()};
        var tableData = await _tableSectionRepository.GetTableListAsync(sectionId, pageNo, pageSize, search);

        model.TableList = tableData.tables;
        model.Page.SetPagination(tableData.totalRecords, pageSize, pageNo);
        return model;
    }

    public async Task<bool> DeleteSection(long id, string userName)
    {
        if(id == 0)
        {
            return false;
        }

        User user = await _userRepository.GetUserByUserName(userName);
        if(user == null)
        {
            return false;
        }

        return await _tableSectionRepository.DeleteSectionAsync(id, user.Id);
    }
}
