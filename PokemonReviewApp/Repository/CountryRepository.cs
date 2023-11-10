using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{

    public class CountryRepository : ICountryRepository
    {
        private DataContext _dbContext;
        public CountryRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<Country> GetCountries()
        {
             return _dbContext.Countries.ToList();
            
        }
        
        public Country GetCountry(int id)
        {
            return _dbContext.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _dbContext.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersByCountry(int categoryId)
        {
            return _dbContext.Countries.Where(c => c.Id == categoryId).FirstOrDefault().Owners;
        }

        public bool IsExisCountry(int countryId)
        {
            return _dbContext.Countries.Any(c => c.Id == countryId);
        }
    }
}
