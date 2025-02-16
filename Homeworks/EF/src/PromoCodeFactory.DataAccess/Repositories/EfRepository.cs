using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T> : IRepository<T> where T: BaseEntity
    {
        protected readonly DbContext Context;
        private readonly DbSet<T> _entitySet;

        public EfRepository(DbContext context)
        {
            Context = context;
            _entitySet = Context.Set<T>();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<T>>(_entitySet);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _entitySet.FindAsync(id);
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, string[] navProps)
        {          
            IQueryable<T> query = _entitySet;
            
            foreach (var entity in navProps)
                query = query.Include(entity);

            return await query.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            return (await _entitySet.AddAsync(entity)).Entity;
        }
        
        public async Task<bool> UpdateAsync(T entity)
        {

;           //  Find(entity.Id);  //  FindIndex(x => x.Id == entity.Id);
            //if (index > -1)
            //{
            //    _data[index] = entity;
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }
        
        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var entity = await _entitySet.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) 
                return false;

            _entitySet.Remove(entity);

            return true;
        }

        /// <summary>
        /// Сохранить изменения.
        /// </summary>
        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        /// <summary>
        /// Сохранить изменения асинхронно
        /// </summary>
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// Найти по условию
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="predicate">Условие поиска</param>
        /// <returns>Интерфейс для запроса данных</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual IQueryable<T> FindAllWhere<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate value must be passed to FindAllBy<T>.");

            return Context.Set<T>().Where(predicate);
        }

    }
}
