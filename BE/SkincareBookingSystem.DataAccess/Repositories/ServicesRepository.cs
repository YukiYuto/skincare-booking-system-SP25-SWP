using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class ServicesRepository : Repository<Services>, IServicesRepository
    {
        private readonly ApplicationDbContext _context; 
        public ServicesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Services target, Services source)
        {
            _context.Set<Services>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
