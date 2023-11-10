using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        ICollection<Owner> GetOwnersByCountry(int categoryId);
        Country GetCountryByOwner(int ownerId);
        bool IsExisCountry(int categoryId);
    }
}
