using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Application.Repositories
{
    public interface ICategoriaRepository: IRepository<Category>
    {
        Task<Category> GetByIdAsync(int id);
        Task UpdateAsync(Category category);
    }
}
