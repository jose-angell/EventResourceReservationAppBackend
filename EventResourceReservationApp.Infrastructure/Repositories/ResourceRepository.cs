using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using EventResourceReservationApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
