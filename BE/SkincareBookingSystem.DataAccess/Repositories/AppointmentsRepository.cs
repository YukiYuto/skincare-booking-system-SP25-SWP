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

        public void Update(Appointments target, Appointments source)
        {
            _context.Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
