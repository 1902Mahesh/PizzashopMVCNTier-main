using System.ComponentModel.DataAnnotations;

namespace DataLogicLayer.ViewModels;

public class ChangePassViewModel
{
    
    [Required(ErrorMessage = "The current password is required")]
    public string  CurrentPassword { get; set; }

    [Required(ErrorMessage = "This field is required")]
    public string  NewPassword { get; set; }

    [Required(ErrorMessage = "This field is required")]
    public string  ConfirmNewPassword { get; set; }
}
