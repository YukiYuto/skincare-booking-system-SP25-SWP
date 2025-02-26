using Microsoft.EntityFrameworkCore;
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

        public void Update(TherapistSchedule target, TherapistSchedule source)
        {
            _context.Set<TherapistSchedule>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
