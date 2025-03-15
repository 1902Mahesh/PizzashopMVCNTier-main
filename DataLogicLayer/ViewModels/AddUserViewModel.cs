using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace DataLogicLayer.ViewModels;

public class AddUserViewModel
{
    // public long UserId { get; set; }
    [Required(ErrorMessage = "First Name is required")]
    [RegularExpression("^[A-Za-z]+$", ErrorMessage = "First Name must contain only letters")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Last Name must contain only letters")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public int RoleId { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password Field is Required")]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain at least one letter, one number, and one special character.")]
    public string Password { get; set; }
    public IFormFile? ProfileImage { get; set; } // For Uploading Image

    // New properties for country, state, and city selection
    [Required(ErrorMessage = "Country is required")]
    public long CountryId { get; set; }
    [Required(ErrorMessage = "State is required")]
    public long StateId { get; set; }
    [Required(ErrorMessage = "City is required")]
    public long CityId { get; set; }

    [Required(ErrorMessage = "Zipcode is required")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Zipcode must be exactly 6 digits")]
    public int Zipcode { get; set; }
    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Phone Number is required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
    public string Phone { get; set; }

}
