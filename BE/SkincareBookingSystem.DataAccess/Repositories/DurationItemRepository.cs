using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class DurationItemRepository : Repository<DurationItem>, IDurationItemRepository
    {
        private readonly ApplicationDbContext _context; 
        public DurationItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
