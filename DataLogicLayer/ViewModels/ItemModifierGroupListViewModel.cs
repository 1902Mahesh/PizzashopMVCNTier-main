namespace DataLogicLayer.ViewModels;

public class ItemModifierGroupListViewModel
{
    public long ItemId { get; set; } 
    public long ModifierGroupId { get; set;}

    public string? Name { get; set; }
    public int? MinAllowed { get; set; }
    public int? MaxAllowed { get; set; }

    public List<ModifierItemViewModel> ModifierItemList  { get; set; } = new List<ModifierItemViewModel>();
}
