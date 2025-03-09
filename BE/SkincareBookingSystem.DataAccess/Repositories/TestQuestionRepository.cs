using Microsoft.EntityFrameworkCore;
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

        public void Update(TestQuestion target, TestQuestion source)
        {
            _context.Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
