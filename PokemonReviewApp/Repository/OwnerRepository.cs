using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private DataContext _context;
        public OwnerRepository(DataContext context) 
        {
            _context = context;
        }

        //public bool CreateOwner(Owner owner, int pokemonId, int countryId)
        //{
        //    var pokemon = _context.Pokemon.Where(p => p.Id == pokemonId).FirstOrDefault();

        //}

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public Owner GetOwnerOfPokemon(int pokemonId)
        {
            return _context.PokemonOwners.Where(p => p.PokemonId == pokemonId).Select(o => o.Owner).FirstOrDefault();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(o => o.OwnerId == ownerId).Select(p=>p.Pokemon).ToList();
        }

        public bool HasOwner(int ownerId)
        {
            return _context.Owners.Any(o => o.Id == ownerId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
