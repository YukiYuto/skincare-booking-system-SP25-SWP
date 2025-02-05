using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class ServiceComboRepository : Repository<ServiceCombo>, IServiceComboRepository
    {
        private readonly ApplicationDbContext _context; 
        public ServiceComboRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
