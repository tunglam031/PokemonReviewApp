using Microsoft.EntityFrameworkCore.Diagnostics;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokeCateRepository : IPokeCateRepository
    {
        private readonly DataContext _context;
        public PokeCateRepository(DataContext context) 
        {
            _context = context;
        }

        public bool DeletePokeCate(PokemonCategory pokeCate)
        {
            _context.Remove(pokeCate);
            return Save();
        }

        public ICollection<PokemonCategory> GetPokeCateByCateId(int cateId)
        {
            return _context.PokemonCategories.Where(pc => pc.CategoryId == cateId).ToList();
        }

        public ICollection<PokemonCategory> GetPokeCateByPokeId(int pokeId)
        {
            return _context.PokemonCategories.Where(pc => pc.PokemonId == pokeId).ToList();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
