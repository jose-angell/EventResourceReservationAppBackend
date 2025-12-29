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
    public class ReservationDetailRepository : Repository<ReservationDetail>, IReservationDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public ReservationDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ReservationDetail?> GetByIdAsync(Guid id)
        {
            return await _context.ReservationDetails
                .Include(r => r.Reservation) 
                .Include(r => r.Resource) 
                .FirstOrDefaultAsync(r => r.Id == id); // Filtrar por Id
        }
        public async Task Update(ReservationDetail reservationDetail)
        {
            _context.ReservationDetails.Update(reservationDetail);
            await Task.CompletedTask;
        }
    }
}
