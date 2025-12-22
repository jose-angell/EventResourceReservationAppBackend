using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventResourceReservationApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Categories { get; private set; }
        public ILocationRepository Locations { get; private set; }
        public IReservationCarItemRepository ReservationCarItems { get; private set; }
        public IReviewRepository Reviews { get; private set; } 
        public IResourceRepository Resources { get; private set; }
        public IReservationRepository Reservations { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Categories = new CategoryRepository(_context);
            Locations = new LocationRepository(_context);
            ReservationCarItems = new ReservationCarItemRepository(_context);
            Reviews = new ReviewRepository(_context);
            Resources = new ResourceRepository(_context);
            Reservations = new ReservationRepository(_context);
        }
        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                //TODO: _logger.LogError(ex, "Ocurrió un error de actualización de base de datos durante SaveAsync.");
                throw new PersistenceException("Un error de base de datos impidió guardar los cambios.", dbEx);
            }
            catch (Exception ex)
            {
                //TODO: _logger.LogError(ex, "Ocurrió un error inesperado durante SaveAsync.");
                throw new PersistenceException("Ocurrió un error inesperado al persistir los cambios.", ex);
            }
        }
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this); // le indica a Garbage Collector que no necesita llamar al finalizador de esta clase
        }
    }
}
