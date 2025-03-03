using System.ComponentModel.DataAnnotations;

namespace DataLogicLayer.ViewModels;

public class ResetPassViewModel
{
    [Required(ErrorMessage = "Password Field is Required")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password Must be of minimum 8 character")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Confirm Password Field is Required")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password Must be of minimum 8 character")]
    [Compare(nameof(Password), ErrorMessage = "Password Do Not Match")]
    public string? ConfirmPassword { get; set; }

    public string? Email { get; set; }
}
