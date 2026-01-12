using EventResourceReservationApp.Application.DTOs.Resources;
using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.Repositories
{
    public interface IResourceRepository: IRepository<Resource>
    {
        Task<Resource> GetByIdAsync(Guid id);
        Task<bool> IsResourceInUse(Guid id);
        Task UpdateAsync(Resource resource);
        Task<IEnumerable<ResourceResponse?>> GetAllAsync(Expression<Func<Resource, bool>> filter, DateTime star, DateTime end);
    }
}
