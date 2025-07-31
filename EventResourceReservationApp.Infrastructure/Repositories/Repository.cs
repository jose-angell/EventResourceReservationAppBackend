using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventResourceReservationApp.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        internal DbSet<T> _dbSet;// DbSet interno para trabajar con la entidad
        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();// Inicializa el DbSet para la entidad T
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (includeProperties != null)
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                if (orderBy != null)
                {
                    return await orderBy(query).ToListAsync();
                }
                return await query.ToListAsync();
            }
            catch (TimeoutException ex)
            {
                //TODO: _logger.LogError(ex, "La consulta GetAllAsync excedió el tiempo de espera.");
                throw new PersistenceException("La consulta de la base de datos excedió el tiempo de espera.", ex);
            }
            catch (Exception ex)
            {
                //TODO: _logger.LogError(ex, "Ocurrió un error inesperado durante GetAllAsync.");
                throw new PersistenceException("Ocurrió un error inesperado al consultar la base de datos.", ex);
            }
        }
        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                query = query.Where(filter);
                if (includeProperties != null)
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                return await query.FirstOrDefaultAsync();
            }
            catch (TimeoutException ex)
            {
                //TODO: _logger.LogError(ex, "La consulta GetAllAsync excedió el tiempo de espera.");
                throw new PersistenceException("La consulta de la base de datos excedió el tiempo de espera.", ex);
            }
            catch (Exception ex)
            {
                //TODO: _logger.LogError(ex, "Ocurrió un error inesperado durante GetAllAsync.");
                throw new PersistenceException("Ocurrió un error inesperado al consultar la base de datos.", ex);
            }
        }

        public async Task RemoveASync(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }
    }
}
