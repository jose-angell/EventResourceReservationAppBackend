using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using EventResourceReservationApp.Infrastructure.Data;

namespace EventResourceReservationApp.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {

        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await Task.CompletedTask;
        }
    }
}
