using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class TherapistScheduleRepository : Repository<TherapistSchedule>, ITherapistScheduleRepository
    {
        private readonly ApplicationDbContext _context; 
        public TherapistScheduleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
