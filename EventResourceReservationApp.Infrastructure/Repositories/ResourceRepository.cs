using EventResourceReservationApp.Application.DTOs.Resources;
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

        public async Task<IEnumerable<ResourceResponse?>> GetAllAsync(Expression<Func<Resource, bool>> filter, DateTime star, DateTime end)
        {
            return await _context.Resources.Where(filter)
                .Include(r => r.CreatedByUser)
                .Include(r => r.Location)
                .Include(r => r.Category)
                .Select(resource => new ResourceResponse
                {
                    Id = resource.Id,
                    StatusId = resource.StatusId,
                    StatusDescription = "",
                    Name = resource.Name,
                    Description = resource.Description,
                    Price = resource.Price,
                    Quantity = resource.Quantity,
                    QuantityInUse = 0, // Aquí debes implementar la lógica para calcular la cantidad en uso
                    //QuantityInUse = resource.Reservations // Asumiendo que Resource tiene ICollection<Reservation>
                    //.Where(reservation =>
                    //    reservation.StartDate < endDate &&
                    //    reservation.EndDate > startDate
                    //)
                    //.Sum(reservation => (int?)reservation.Quantity) ?? 0,
                    CategoryId = resource.CategoryId,
                    CategoryName = resource.Category != null ? resource.Category.Name : "",
                    LocationId = resource.LocationId,
                    LocationDescription = resource.Location != null ? resource.Location.City : "",
                    AuthorizationType = resource.AuthorizationType,
                    Created = resource.CreatedAt
                })
                .ToListAsync();
        }
    }
}
