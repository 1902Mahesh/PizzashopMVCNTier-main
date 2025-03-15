using System.ComponentModel.DataAnnotations;
using DataLogicLayer.Models;
using Microsoft.AspNetCore.Http;

namespace DataLogicLayer.ViewModels;

public class AdditemViewModel
{
    public long ItemId { get; set; }

    [Required(ErrorMessage = "Item name is required")]
    public string Name {get; set; }

    [Required(ErrorMessage ="Rate is Required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Rate must be greater than zero")]
    public decimal Rate { get; set; }

    [Required(ErrorMessage ="Quantity is Required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    public bool IsAvailable { get; set; } = false;
    public bool DefaultTax { get; set; } = false;
    public long TaxId { get; set; }

    [Range(0, 100, ErrorMessage = "Tax percentage must be between 0 and 100")]
    public decimal TaxPercentage { get; set; }
    public string? ShortCode { get; set; }

    public string? Description { get; set; }

    public IFormFile? ItemImage { get; set; }

    public string? ImgUrl { get; set; }


    // Id's For the Child Tables
    [Required(ErrorMessage = "Category is required")]
    [Range(1, long.MaxValue, ErrorMessage = "Category is required")]
    public long CategoryId { get; set; }
    [Required(ErrorMessage = "Item Type is required")]
    [Range(1, long.MaxValue, ErrorMessage = "Item Type is required")]
    public long ItemTypeId { get; set; }
    [Required(ErrorMessage = "Unit is required")]
    [Range(1, long.MaxValue, ErrorMessage = "Unit is required")]
    public long UnitId { get; set; }

    public List<CategoryViewModel> CategoryList { get; set; } = new List<CategoryViewModel>();
    public List<Itemtype> ItemTypeList { get; set; } = new List<Itemtype>();
    public List<Unit> UnitList { get; set; } = new List<Unit>();
    public IEnumerable<ModifierGroupViewModel>? ModifierGropList { get; set; }
    public List<ItemModifierGroupListViewModel>? ItemModifierList { get; set; }
 
}
