using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using EventResourceReservationApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Infrastructure.Repositories
{
    public class ReservationCarItemRepository: Repository<ReservationCarItem>, IReservationCarItemRepository
    {
        private readonly ApplicationDbContext _context;
        public ReservationCarItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ReservationCarItem> GetByIdAsync(Guid id)
        {
            return await _context.ReservationCarItems.FindAsync(id);
        }
        public async Task UpdateAsync(ReservationCarItem item)
        {
            _context.ReservationCarItems.Update(item);
            await Task.CompletedTask;
        }
    }
}
