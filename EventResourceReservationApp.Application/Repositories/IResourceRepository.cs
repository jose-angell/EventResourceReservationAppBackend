using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.Repositories
{
    public interface IResourceRepository: IRepository<Resource>
    {
        Task<Resource> GetByIdAsync(Guid id);
        Task UpdateAsync(Resource resource);
    }
}
