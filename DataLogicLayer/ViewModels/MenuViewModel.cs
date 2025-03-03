namespace DataLogicLayer.ViewModels;

public class MenuViewModel
{
    public List<CategoryViewModel> CategoryList { get; set; } = new List<CategoryViewModel>();

    public CategoryViewModel Category { get; set; }
}
