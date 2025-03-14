using DataLogicLayer.Models;

namespace DataLogicLayer.ViewModels;

public class ModifierItemViewModel
{
    public long ModifierItemId { get; set; }
    public string Name { get; set; }
    public decimal? Rate { get; set; }
    public string Unit { get; set; }

    public int Quantity { get; set; }
}
