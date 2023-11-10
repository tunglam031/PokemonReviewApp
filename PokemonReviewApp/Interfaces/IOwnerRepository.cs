using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        Owner GetOwnerOfPokemon(int pokemonId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        bool HasOwner(int ownerId);
        //bool CreateOwner(Owner owner);
        bool Save();
    }
}
