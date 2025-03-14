using DataLogicLayer.Models;

namespace DataLogicLayer.ViewModels;

public class AddEditModifierViewModel
{
    public long ModifierItemId { get; set; } = 0;
    public long ModifierGroupId { get; set; }
    public long OldModifierGroupId { get; set; } = 0;
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal? Rate { get; set; } = 0;
    public int Quantity { get; set; } = 0;

    public long UnitId { get; set; }
    public List<Unit> UnitList { get; set; } = new List<Unit>();

    public IEnumerable<ModifierGroupViewModel>? ModifierGroupList { get; set; }
}
