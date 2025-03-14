using Microsoft.AspNetCore.Http;
namespace DataLogicLayer.ViewModels;

public class AddUserViewModel
{
    // public long UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public int RoleId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public IFormFile? ProfileImage { get; set; } // For Uploading Image

    // New properties for country, state, and city selection
    public long CountryId { get; set; }
    public long StateId { get; set; }
    public long CityId { get; set; }

    public int Zipcode { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }

}
