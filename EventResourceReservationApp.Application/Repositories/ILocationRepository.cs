using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.Repositories
{
    public interface ILocationRepository: IRepository<Location>
    {
        Task<Location> GetByIdAsync(int id);
        Task UpdateAsync(Location location);
    }
}
