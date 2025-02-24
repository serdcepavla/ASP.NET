using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Repositories;

public class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly DbContext Context;
    private readonly DbSet<T> _entitySet;
    
    public EfRepository(DatabaseContext context)
    {
        Context = context;
        _entitySet = Context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Task.FromResult<IEnumerable<T>>(_entitySet);
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _entitySet.FindAsync(id);
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        if (entity == null)
            return false;

        _entitySet.Remove(entity);
        await Context.SaveChangesAsync();
        return true;
    }

    public async Task<T> CreateAsync(T entity)
    {
        var result = await _entitySet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        if (entity == null)
            return false;

        Context.Entry(entity).State = EntityState.Modified;
        await Context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
    {
        var entities = await Context.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
        return entities;
    }
}
