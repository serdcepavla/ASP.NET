using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        //Task<IQueryable<T>> GetAllAsync(string[] navProps);

        Task<T> GetByIdAsync(Guid id);
        Task<T> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteByIdAsync(Guid id);
        void SaveChanges();
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}