using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces.Base;
using System.Linq.Expressions;

namespace PokemonReviewApp.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal DataContext context;
        internal DbSet<T> dbSet;
        public GenericRepository(DataContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(object id)
        {
            throw new NotImplementedException();
        }
    }
}

