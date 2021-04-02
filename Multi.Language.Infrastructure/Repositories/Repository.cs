using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Multi.Language.Domain.SeedWork;

namespace Multi.Language.Infrastructure.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly UserContext DataBaseContext;
        protected Repository(UserContext context)
        {
            DataBaseContext = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return DataBaseContext.Set<TEntity>();
        }

        public virtual async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await DataBaseContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await DataBaseContext.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match)
        {
            return await DataBaseContext.Set<TEntity>().SingleOrDefaultAsync(match);
        }

        public ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> match)
        {
            return DataBaseContext.Set<TEntity>().Where(match).ToList();
        }

        public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match)
        {
            return await DataBaseContext.Set<TEntity>().Where(match).ToListAsync();
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await Task.Run(() =>
            {
                DataBaseContext.Set<TEntity>().Attach(entity);
                DataBaseContext.Entry(entity).State = EntityState.Modified;
            });
            return entity;
        }

        public void Remove(TEntity entity)
        {
            DataBaseContext.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = GetAll();
            foreach (var includeProperty in includeProperties)
            {
                queryable = queryable.Include(includeProperty);
            }
            return queryable;
        }
    }
}
