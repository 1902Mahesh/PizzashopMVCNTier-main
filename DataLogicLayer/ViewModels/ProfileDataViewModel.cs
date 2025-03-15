using System;
using Microsoft.AspNetCore.Http;
using DataLogicLayer.Models;
using System.ComponentModel.DataAnnotations;
namespace DataLogicLayer.ViewModels;

public class ProfileDataViewModel
{
    public string? Email { get; set; }
    [Required(ErrorMessage = "First Name is required")]
    [RegularExpression("^[A-Za-z]+$", ErrorMessage = "First Name must contain only letters")]
    public string firstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Last Name must contain only letters")]
    public string lastName { get; set; }

    [Required(ErrorMessage = "Username is required")]

    public string userName { get; set; }

    [Required(ErrorMessage = "Phone Number is required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
    public string phoneNo { get; set; }

    [Required(ErrorMessage = "Zipcode is required")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Zipcode Code must be exactly 6 digits")]
    public int zipcode { get; set; }
    [Required(ErrorMessage = "Address is required")]
    public string address { get; set; }
    public string? Role { get; set; }

    public string? Imgurl { get; set; } = null!;
    public IFormFile? ProfileImage { get; set; } // For Uploading Image


    // New properties for country, state, and city selection
    [Required(ErrorMessage = "Country is required")]
    public long CountryId { get; set; }

    [Required(ErrorMessage = "State is required")]
    public long StateId { get; set; }

    [Required(ErrorMessage = "City is required")]
    public long CityId { get; set; }

    public List<Country>? Countries { get; set; } = new List<Country>();
    public List<State>? States { get; set; } = new List<State>();
    public List<City>? Cities { get; set; } = new List<City>();
}