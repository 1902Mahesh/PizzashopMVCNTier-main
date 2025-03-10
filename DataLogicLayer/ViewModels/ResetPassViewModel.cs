using System.ComponentModel.DataAnnotations;

namespace DataLogicLayer.ViewModels;

public class ResetPassViewModel
{
    [Required(ErrorMessage = "Password Field is Required")]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain at least one letter, one number, and one special character.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Confirm Password Field is Required")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password Must be of minimum 8 character")]
    
    [Compare(nameof(Password), ErrorMessage = "Password Do Not Match")]
    public string? ConfirmPassword { get; set; }

    public string? Token { get; set; }
}
