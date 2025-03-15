using System.ComponentModel.DataAnnotations;
using DataLogicLayer.Models;

namespace DataLogicLayer.ViewModels;

public class AddEditModifierViewModel
{
    public long ModifierItemId { get; set; } = 0;

    [Required(ErrorMessage = "Modifier Group is required")]
    [Range(1, long.MaxValue, ErrorMessage = "Modifier Group is required")]
    public long ModifierGroupId { get; set; }
    public long OldModifierGroupId { get; set; } = 0;

    [Required(ErrorMessage = "Modifier name is required")]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Required(ErrorMessage ="Rate is Required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Rate must be greater than zero")]
    public decimal? Rate { get; set; }

    [Required(ErrorMessage ="Quantity is Required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
    
    [Required(ErrorMessage = "Unit is required")]
    [Range(1, long.MaxValue, ErrorMessage = "Unit is required")]
    public long UnitId { get; set; }
    public List<Unit> UnitList { get; set; } = new List<Unit>();

    public IEnumerable<ModifierGroupViewModel>? ModifierGroupList { get; set; }
}
