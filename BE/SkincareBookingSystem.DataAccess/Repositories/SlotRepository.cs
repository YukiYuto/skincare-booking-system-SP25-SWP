using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class SlotRepository : Repository<Slot>, ISlotRepository
    {
        private readonly ApplicationDbContext _context; 
        public SlotRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
