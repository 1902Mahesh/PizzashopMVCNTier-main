using System.ComponentModel.DataAnnotations;

namespace DataLogicLayer.ViewModels;

public class ForgotPassViewModel
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is Required")]
    public string Email { get; set; }
}
