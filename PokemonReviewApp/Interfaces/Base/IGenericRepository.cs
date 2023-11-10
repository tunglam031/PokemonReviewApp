using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using System.Linq;
using System.Linq.Expressions;

namespace PokemonReviewApp.Interfaces.Base
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
    }
}
