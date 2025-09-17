using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.Repositories
{
    public interface IUnitOfWork: IDisposable
    {
        ICategoryRepository Categories { get; }
        ILocationRepository Locations { get; }
        IReservationCarItemRepository ReservationCarItems { get; }
        IReviewRepository Reviews { get; }
        Task SaveAsync();
    }
}
