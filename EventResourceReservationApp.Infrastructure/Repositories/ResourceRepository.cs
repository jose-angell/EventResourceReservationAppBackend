using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using EventResourceReservationApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EventResourceReservationApp.Infrastructure.Repositories
{
    public class ResourceRepository: Repository<Resource>, IResourceRepository
    {
        private readonly ApplicationDbContext _context;
        public ResourceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Resource?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(r => r.CreatedByUser) // Incluir el usuario que creó el recurso
                .Include(r => r.Location) // Incluir la ubicación del recurso
                .Include(r => r.Category) // Incluir la categoría del recurso
                .FirstOrDefaultAsync(r => r.Id == id); // Filtrar por Id
        }
        public async Task UpdateAsync(Resource resource)
        {
            _context.Resources.Update(resource);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Resource?>> GetAllAsync(Expression<Func<Resource, bool>> filter, DateTime star, DateTime end)
        {
            IQueryable<Resource> query = _dbSet;
            query = query.Where(filter);
            return await query
                .Include(r => r.CreatedByUser)
                .Include(r => r.Location)
                .Include(r => r.Category).ToListAsync();
            /*TODO: Implementar el caldulo de sumar las cantidades de las reservas activas en el rango de fechas proporcionado
             * QuantityInUse = resource.Reservations // Accede a la colección de Reservas (si la tienes)
                .Where(reservation =>
                    // Condición 1: La reserva debe iniciar antes de que termine el rango SOLICITADO
                    reservation.StartDate < endDate && 
                    // Condición 2: La reserva debe terminar después de que inicie el rango SOLICITADO
                    reservation.EndDate > startDate
                )
                .Sum(reservation => (int?)reservation.Quantity) ?? 0 
                // Usamos (int?) para manejar el caso de que la suma sea NULL (0 reservas)
             */
        }
    }
}
