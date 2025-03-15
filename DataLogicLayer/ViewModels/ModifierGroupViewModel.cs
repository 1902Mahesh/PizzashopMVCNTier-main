using System.ComponentModel.DataAnnotations;

namespace DataLogicLayer.ViewModels;

public class ModifierGroupViewModel
{
    public long ModifierId { get; set; }
    [Required(ErrorMessage ="Name is Required")]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<ModifierItemViewModel> ModifierItemList { get; set; } = new List<ModifierItemViewModel>();
}
