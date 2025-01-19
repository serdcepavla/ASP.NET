using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public abstract class EfRepository<T> : IRepository<T> where T: BaseEntity
    {
        protected readonly DbContext Context;
        private readonly DbSet<T> _entitySet;

        protected EfRepository(DbContext context)
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


        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
