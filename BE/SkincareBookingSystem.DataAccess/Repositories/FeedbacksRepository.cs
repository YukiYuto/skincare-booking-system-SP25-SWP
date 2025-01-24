using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class FeedbacksRepository : Repository<Feedbacks>, IFeedbacksRepository
    {
        private readonly ApplicationDbContext _context; 
        public FeedbacksRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
