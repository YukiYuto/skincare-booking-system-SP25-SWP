using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class BlogCategoryRepository : Repository<BlogCategory>, IBlogCategoryRepository
    {
        private readonly ApplicationDbContext _context; 
        public BlogCategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
