using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    /// <summary>
    /// Репозиторий работы с сотрудниками.
    /// </summary>
    public class EmployeeRepository : EfRepository<Employee>, IRepository<Employee>
    {
        public EmployeeRepository(DatabaseContext context) : base(context) { }

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            var query = Context.Set<Employee>().AsQueryable();
            query = query.Where(e => e.Id == id).Include(e => e.Role);
            return await query.SingleOrDefaultAsync(); // _entitySet.FindAsync(id);
        }
    }
}
