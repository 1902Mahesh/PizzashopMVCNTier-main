using System;
using Microsoft.AspNetCore.Http;
using DataLogicLayer.Models;
namespace DataLogicLayer.ViewModels;

public class ProfileDataViewModel
{
    public string? Email { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string userName { get; set; }
    public string phoneNo { get; set; }
    public int zipcode { get; set; }
    public string address { get; set; }

    public string? Role { get; set; }

    public string? Imgurl { get; set; } = null!;
    public IFormFile? ProfileImage { get; set; } // For Uploading Image


    // New properties for country, state, and city selection
    public long CountryId { get; set; }
    public long StateId { get; set; }
    public long CityId { get; set; }



    public List<Country>? Countries { get; set; } = new List<Country>();
    public List<State>? States { get; set; } = new List<State>();
    public List<City>? Cities { get; set; } = new List<City>();
}