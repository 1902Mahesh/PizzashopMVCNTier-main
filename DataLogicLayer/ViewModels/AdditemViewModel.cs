using DataLogicLayer.Models;
using Microsoft.AspNetCore.Http;

namespace DataLogicLayer.ViewModels;

public class AdditemViewModel
{
    public long ItemId { get; set; }

    public string Name {get; set; }

    public decimal Rate { get; set; } = 0;
    public int Quantity { get; set; } = 0;

    public bool IsAvailable { get; set; } = false;
    public bool DefaultTax { get; set; } = false;

    public decimal TaxPercentage { get; set; }
    public string? ShortCode { get; set; }

    public string? Description { get; set; }

    public IFormFile? ItemImage { get; set; }

    public string? ImgUrl { get; set; }


    // Id's For the Child Tables
    public long CategoryId { get; set; }
    public long ItemTypeId { get; set; }
    public long UnitId { get; set; }

    public List<CategoryViewModel> CategoryList { get; set; } = new List<CategoryViewModel>();
    public List<Itemtype> ItemTypeList { get; set; } = new List<Itemtype>();
    public List<Unit> UnitList { get; set; } = new List<Unit>();
    public IEnumerable<ModifierGroupViewModel>? ModifierGropList { get; set; }
    public List<ItemModifierGroupListViewModel>? ItemModifierList { get; set; }
 
}
