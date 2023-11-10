using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokeOwnerRepository
    {
        ICollection<PokemonOwner> GetPokeOwnerByPokeId(int pokeId);
        ICollection<PokemonOwner> GetPokeOwnerByOwnerId(int pokeId);
        bool DeletePokeOwner(PokemonOwner pokeOwner);
        bool Save();
    }
}
