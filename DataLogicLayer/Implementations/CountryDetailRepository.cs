using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;

namespace DataLogicLayer.Implementations;

public class CountryDetailRepository : ICountryDetailRepository
{
    private readonly PizzaShopDbContext _context;
    public CountryDetailRepository(PizzaShopDbContext context)
    {
        _context = context;
    }


    /*---------------------------------------------------------------------------Get Country Method Implementation
    -------------------------------------------------------------------------------------------------------*/
    public List<Country> GetCountry()
    {
        return _context.Countries.ToList();
    }

    /*---------------------------------------------------------------------------Get State Method Implementation
    -------------------------------------------------------------------------------------------------------*/
    public List<State> GetState(long countryId)
    {
        return _context.States.Where(u => u.Countryid == countryId).ToList();
    }

    /*---------------------------------------------------------------------------Get City Method Implementation
    -------------------------------------------------------------------------------------------------------*/
    public List<City> GetCity(long id)
    {
        return _context.Cities.Where(u => u.Stateid == id).ToList();
    }

}
