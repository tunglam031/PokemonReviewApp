using PokemonReviewApp.Models;
using PokemonReviewApp.Models.Filter;
using System.Globalization;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories(CategoryFilter? filter=null);
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonsByCategory(int categoryId);
        bool IsExisCategory(int categoryId);
        bool CreateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();


    }
}
