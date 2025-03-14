using System.ComponentModel.DataAnnotations;

namespace DataLogicLayer.ViewModels;

public class CategoryViewModel
{
    public long Id { get; set; } = -1;
    public string CategoryName { get; set; } = null!;
    public string? Description { get; set; }
}
