namespace DataLogicLayer.ViewModels;

public class ItemListViewModel
{
    public long? CategoryId { get; set; }
    public IEnumerable<ItemsViewModel>? ItemList { get; set; }

    public PaginationViewModel? Page { get; set; }
}
