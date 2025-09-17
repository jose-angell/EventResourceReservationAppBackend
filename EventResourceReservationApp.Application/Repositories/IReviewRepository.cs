using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.Repositories
{
    public interface IReviewRepository: IRepository<Review>
    {
        Task<Review> GetById(Guid id);
        Task UpdateAsync(Review review);
    }
}
