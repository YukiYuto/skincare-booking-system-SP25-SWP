using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class SkinTestRepository : Repository<SkinTest>, ISkinTestRepository
    {
        private readonly ApplicationDbContext _context; 
        public SkinTestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
