using Microsoft.AspNetCore.Http;
using DataLogicLayer.Models;

namespace DataLogicLayer.ViewModels;

public class EditUserViewModel
{
    public long UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public long RoleId { get; set; }
    public string Email { get; set; }

    public bool Status { get; set; }

    public IFormFile? ProfileImage { get; set; } // For Uploading Image

    public string? ImgUrl { get; set; }

    public long CountryId { get; set; }
    public long StateId { get; set; }
    public long CityId { get; set; }

    public List<Role> Roles { get; set; } = new List<Role>();
    public List<Country> Countries { get; set; } = new List<Country>();
    public List<State> States { get; set; } = new List<State>();
    public List<City> Cities { get; set; } = new List<City>();

    public int Zipcode { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }

}
