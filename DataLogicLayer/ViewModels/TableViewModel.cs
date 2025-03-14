namespace DataLogicLayer.ViewModels;

public class TableViewModel
{

    public long? SectionId { get; set; }
    public long TableId { get; set; }
    public string TableName { get; set; }

    public int Capacity { get; set; }
    public bool IsOccupied { get; set; }
}
