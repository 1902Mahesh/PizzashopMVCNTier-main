using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;

namespace BusinessLogicLayer.Implementations;

public class CountryService : ICountryService
{
    private readonly ICountryDetailRepository _countryDetailRepository;


    public CountryService(ICountryDetailRepository countryDetailRepository)
    {
        _countryDetailRepository = countryDetailRepository;

    }


    /*-------------------------------------------------------------------------------------------------------------Get Country Service Implementation
    -----------------------------------------------------------------------------------------------------------------------------------------*/
    public List<City> GetCities(long id)
    {
        return _countryDetailRepository.GetCity(id);
    }

    /*-------------------------------------------------------------------------------------------------------------Get States Service Implementation
    -----------------------------------------------------------------------------------------------------------------------------------------*/
    public List<Country> GetCountries()
    {
        return _countryDetailRepository.GetCountry();
    }

    /*-------------------------------------------------------------------------------------------------------------Get Cities Service Implementation
    -----------------------------------------------------------------------------------------------------------------------------------------*/
    public List<State> GetStates(long countryid)
    {
        return _countryDetailRepository.GetState(countryid);
    }

}
