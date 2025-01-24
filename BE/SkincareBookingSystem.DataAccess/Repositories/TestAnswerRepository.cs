using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class TestAnswerRepository : Repository<TestAnswer>, ITestAnswerRepository
    {
        private readonly ApplicationDbContext _context; 
        public TestAnswerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
