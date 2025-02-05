using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class ServiceTypeRepository : Repository<ServiceType>, IServiceTypeRepository
    {
        private readonly ApplicationDbContext _context; 
        public ServiceTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
