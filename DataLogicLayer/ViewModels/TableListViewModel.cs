namespace DataLogicLayer.ViewModels;

public class TableListViewModel
{
    public IEnumerable<TableViewModel>? TableList { get; set; }
    public PaginationViewModel? Page { get; set; }
}
