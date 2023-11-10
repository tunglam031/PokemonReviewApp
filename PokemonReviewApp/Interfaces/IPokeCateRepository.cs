using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokeCateRepository
    {
        ICollection<PokemonCategory> GetPokeCateByPokeId(int pokeId);
        ICollection<PokemonCategory> GetPokeCateByCateId(int pokeId);
        bool DeletePokeCate(PokemonCategory pokeCate);
        bool Save();
    }
}
