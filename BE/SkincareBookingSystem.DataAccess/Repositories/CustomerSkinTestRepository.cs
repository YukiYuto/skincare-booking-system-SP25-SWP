using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class CustomerSkinTestRepository : Repository<CustomerSkinTest>, ICustomerSkinTestRepository
    {
        private readonly ApplicationDbContext _context; 
        public CustomerSkinTestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
