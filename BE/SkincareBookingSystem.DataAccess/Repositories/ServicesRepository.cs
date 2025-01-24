using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class ServicesRepository : Repository<Services>, IServicesRepository
    {
        private readonly ApplicationDbContext _context; 
        public ServicesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
