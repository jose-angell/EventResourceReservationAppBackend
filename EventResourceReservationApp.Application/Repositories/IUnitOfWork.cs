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

        Task SaveAsync();
    }
}
