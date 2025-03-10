using DataLogicLayer.Models;

namespace DataLogicLayer.Interfaces;

public interface ICountryDetailRepository
{
    public List<Country> GetCountry();
    public List<State> GetState(long countryId);
    public List<City> GetCity(long id);
}
