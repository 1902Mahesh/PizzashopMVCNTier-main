namespace DataLogicLayer.ViewModels;

public class ItemsViewModel
{
    public long ItemId { get; set; }

    public string ItemName { get; set; }

    public string ItemType {get; set; }

    public string ItemImg { get; set; }

    public decimal Rate { get; set; }

    public int Quantity { get; set; }

    public bool IsAvailable { get; set; }
}
