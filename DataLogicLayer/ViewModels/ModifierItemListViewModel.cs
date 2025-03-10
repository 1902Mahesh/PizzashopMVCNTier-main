using DataLogicLayer.Models;

namespace DataLogicLayer.ViewModels;

public class ModifierItemListViewModel
{
    public IEnumerable<ModifierItemViewModel>? ModifierItemList { get; set; }
    public PaginationViewModel? Page { get; set; }
}
