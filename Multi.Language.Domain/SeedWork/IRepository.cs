using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Multi.Language.Domain.SeedWork
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        Task<ICollection<T>> GetAllAsync();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<T> UpdateAsync(T entity);
        void Remove(T entity);
    }
}
