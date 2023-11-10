using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Models.Filter;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }


        public ICollection<Category> GetCategories(CategoryFilter? filter = null)
        {
            var queryable = _context.Categories.AsQueryable();
            queryable = AddFilterOnQuery(filter, queryable);
            return queryable.OrderBy(c => c.Id).ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonsByCategory(int categoryId)
        {
            return _context.PokemonCategories.Where(c => c.CategoryId == categoryId).Select( p => p.Pokemon).ToList();
        }

        public bool IsExisCategory(int categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public IQueryable<Category> AddFilterOnQuery(CategoryFilter? filter, IQueryable<Category> queryable)
        {
            if (!string.IsNullOrEmpty(filter?.Name))
            {
                queryable =  queryable.Where(c => c.Name == filter.Name);
            }
            return queryable;
        }
    }
}
