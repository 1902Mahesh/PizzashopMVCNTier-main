namespace DataLogicLayer.ViewModels;

public class MenuViewModel
{
    public List<CategoryViewModel>? CategoryList { get; set; } = new List<CategoryViewModel>();

    public CategoryViewModel? Category { get; set; }

    public ItemListViewModel? ItemModel {get; set; }

    public AdditemViewModel? AddItems { get; set; }
}
