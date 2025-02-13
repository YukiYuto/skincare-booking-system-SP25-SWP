using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class TestQuestionRepository : Repository<TestQuestion>, ITestQuestionRepository
    {
        private readonly ApplicationDbContext _context; 
        public TestQuestionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
