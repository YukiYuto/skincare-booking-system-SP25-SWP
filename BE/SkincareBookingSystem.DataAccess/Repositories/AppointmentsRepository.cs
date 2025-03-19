using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class AppointmentsRepository : Repository<Appointments>, IAppointmentsRepository
    {
        private readonly ApplicationDbContext _context; 
        public AppointmentsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Appointments>> GetAppointmentsByDateAsync(Guid customerId, DateTime date, string? includeProperties = null)
        {
            var query = _context.Appointments.AsQueryable();

            // Filter by customer ID and date
            query = query.Where(a => a.CustomerId == customerId && a.AppointmentDate == DateOnly.FromDateTime(date));

            // Include related properties
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.ToListAsync();
        }

        public void Update(Appointments target, Appointments source)
        {
            _context.Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
        
        public void UpdateStatus(Appointments appointments)
        {
            _context.Appointments.Update(appointments);
        }
    }
}
