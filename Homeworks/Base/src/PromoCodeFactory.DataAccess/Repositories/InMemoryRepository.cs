using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>: IRepository<T> where T: BaseEntity
    {
        protected List<T> _data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            _data = data.ToList();
        }

        public Task<List<T>> GetAllAsync()
        {
            return Task.FromResult(_data);
        }
#nullable enable
        public Task<T?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_data.FirstOrDefault(x => x.Id == id));
        }
#nullable disable
        public bool DeleteByIdAsync(Guid id)
        {
           
            T itemForDelete = Task.FromResult(_data.FirstOrDefault(x => x.Id == id)).Result;
            if (itemForDelete != null)
            {
                _data.Remove(itemForDelete);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CreateAsync(T entity)
        {
            if (entity != null)
            {
                entity.Id = Guid.NewGuid();
                _data.Add(entity);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateAsync(T entity)
        {
            int index = _data.FindIndex(x => x.Id == entity.Id);
            if (index > -1)
            {
                _data[index] = entity;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}