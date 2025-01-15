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

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_data.FirstOrDefault(x => x.Id == id));
        }

        public bool DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}