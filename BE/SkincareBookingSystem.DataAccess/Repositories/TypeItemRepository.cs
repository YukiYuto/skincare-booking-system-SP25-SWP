using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class TypeItemRepository : Repository<TypeItem>, ITypeItemRepository
    {
        private readonly ApplicationDbContext _context; 
        public TypeItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
