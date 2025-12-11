using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter,
            string? includeProperties = null);
    }
}
