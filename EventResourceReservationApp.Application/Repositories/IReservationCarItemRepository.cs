using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.Repositories
{
    public interface IReservationCarItemRepository: IRepository<ReservationCarItem>
    {
        Task<ReservationCarItem> GetByIdAsync(Guid id);
        Task UpdateAsync(ReservationCarItem item);
    }
}
