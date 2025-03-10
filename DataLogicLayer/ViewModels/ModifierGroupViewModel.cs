namespace DataLogicLayer.ViewModels;

public class ModifierGroupViewModel
{
    public long ModifierId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<ModifierItemViewModel> ModifierItemList { get; set; } = new List<ModifierItemViewModel>();
}
