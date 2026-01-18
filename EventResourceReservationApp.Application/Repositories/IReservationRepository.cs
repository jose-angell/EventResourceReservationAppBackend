using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.Repositories
{
    public interface IReservationRepository: IRepository<Reservation>
    {
        Task<Reservation> GetByIdAsync(Guid id);
        Task UpdateAsync(Reservation reservation);
    }
}
