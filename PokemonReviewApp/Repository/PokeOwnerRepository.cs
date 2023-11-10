using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokeOwnerRepository : IPokeOwnerRepository
    {
        private readonly DataContext _context;
        public PokeOwnerRepository(DataContext context)
        {
            _context = context;
        }

        public bool DeletePokeOwner(PokemonOwner pokeOwner)
        {
            _context.Remove(pokeOwner);
            return Save();
        }

        public ICollection<PokemonOwner> GetPokeOwnerByOwnerId(int ownerId)
        {
            return _context.PokemonOwners.Where(po => po.PokemonId == ownerId).ToList();
        }

        public ICollection<PokemonOwner> GetPokeOwnerByPokeId(int pokeId)
        {
            return _context.PokemonOwners.Where(pc => pc.OwnerId == pokeId).ToList();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
