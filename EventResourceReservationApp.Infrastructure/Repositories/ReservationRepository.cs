using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using EventResourceReservationApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Infrastructure.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        private readonly ApplicationDbContext _context;
        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Reservation> GetByIdAsync(Guid id)
        {
            return await _context.Reservations
                .Include(r => r.Client) // Incluir el usuario que creó el recurso
                .Include(r => r.Location) // Incluir la ubicación del recurso
                .Include(r => r.Admin) 
                .Include(r => r.ReservationDetail)
                .FirstOrDefaultAsync(r => r.Id == id); // Filtrar por Id
        }
        public async Task Update(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await Task.CompletedTask;
        }
    }
}
