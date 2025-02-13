using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class ComboItemRepository : Repository<ComboItem>, IComboItemRepository
    {
        private readonly ApplicationDbContext _context; 
        public ComboItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
