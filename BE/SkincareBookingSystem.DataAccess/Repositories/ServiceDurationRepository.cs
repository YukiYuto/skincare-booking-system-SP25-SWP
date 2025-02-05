using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class ServiceDurationRepository : Repository<ServiceDuration>, IServiceDurationRepository
    {
        private readonly ApplicationDbContext _context; 
        public ServiceDurationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
