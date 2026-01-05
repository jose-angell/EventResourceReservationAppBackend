using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.Repositories
{
    public interface IReservationDetailRepository: IRepository<ReservationDetail>
    {
        Task<ReservationDetail> GetByIdAsync(Guid id);
        Task Update(ReservationDetail reservationDetail);
    }
}
