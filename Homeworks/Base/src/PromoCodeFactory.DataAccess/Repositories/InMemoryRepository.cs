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
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T>? GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }
        
        public Task<T> DeleteByIdAsync(Guid id)
        {
            var itemForDelete = Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
            if (itemForDelete != null)
            {
                Data = Data.Where(x => x.Id != id);
            }
            return itemForDelete;
        }

        public Task<T> CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            Data = Data.Append(entity);
            return Task.FromResult(entity);
        }

        public Task<T> UpdateAsync(T entity) 
        {
            var dataList = Data.ToList();

            var data = dataList.FirstOrDefault(x => x.Id == entity.Id);
            if (data == null)
                return null;

            int index = dataList.IndexOf(data);
            dataList[index] = entity;
            Data = dataList;
            return Task.FromResult(entity);
        }
    }
}