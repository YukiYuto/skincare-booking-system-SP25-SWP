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
    }
}
