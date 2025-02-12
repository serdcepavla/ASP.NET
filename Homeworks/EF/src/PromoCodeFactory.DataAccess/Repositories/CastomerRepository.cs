using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Context;

namespace PromoCodeFactory.DataAccess.Repositories;

public class CastomerRepository: EfRepository<Customer>, IRepository<Customer>
{
    public CastomerRepository(DatabaseContext context) : base(context) { }

    // public Task<IEnumerable<Customer>> GetAllAsync()
    // {
    //     return Task.FromResult<IEnumerable<T>>(_entitySet);
    // }
    //public Task<IQueryable<Customer>> GetAllAsync()
    // {
    //     var query = Context.Set<Customer>()
    //         .Include(e => e.PromoCodes).Include(e => e.Preferences);
    //     return Task.FromResult(query); 
        
    //     //return Task.FromResult<IEnumerable<Customer>>(Context.Set<Customer>().Where);
    // }
    // public Task<IQueryable<T>> GetAllAsync(string[] navProps)//Task<IEnumerable<T>>
    // {
    //     IQueryable<T> query = _entitySet;

    //     foreach (var entity in navProps)
    //         query = query.Include(entity);

    //     return Task.FromResult(query);
    // }

    
    // public override async Task<Customer> GetByIdAsync(Guid id)
    // {
    //     var query = Context.Set<Customer>().AsQueryable();
    //     query = query.Where(e => e.Id == id).Include(e => e.PromoCodes).Include(e => e.Preferences);
    //     return await query.SingleOrDefaultAsync(); // _entitySet.FindAsync(id);
    // }
}
