using Microsoft.EntityFrameworkCore;
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

        public void Update(Feedbacks target, Feedbacks source)
        {
            _context.Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
