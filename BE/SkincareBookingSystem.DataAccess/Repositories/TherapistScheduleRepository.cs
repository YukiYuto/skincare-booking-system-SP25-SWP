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

        public void UpdateStatus(TherapistSchedule therapistSchedule)
        {
            throw new NotImplementedException();
        }


        //public async Task<TherapistSchedule?> GetTherapistScheduleByTherapistIdAsync(Guid therapistId, string? includeProperties = null)
        //{
        //    var query = _context.TherapistSchedules.AsQueryable();
        //    if (therapistId != Guid.Empty)
        //    {
        //        query = query.Where(ts => ts.TherapistId == therapistId);
        //    }
        //    if (!string.IsNullOrEmpty(includeProperties))
        //    {
        //        foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            query = query.Include(property);
        //        }
        //    }
        //    return await query.FirstOrDefaultAsync();
        //}
    }
}
