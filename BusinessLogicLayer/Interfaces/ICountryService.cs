using DataLogicLayer.Models;

namespace DataLogicLayer.Interfaces;

public interface ICountryService
{
    public List<Country> GetCountries();
    public List<State> GetStates(long countryid);
    public List<City> GetCities(long id);
}
