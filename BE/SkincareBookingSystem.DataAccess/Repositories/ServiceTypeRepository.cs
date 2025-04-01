using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class ServiceTypeRepository : Repository<ServiceType>, IServiceTypeRepository
    {
        private readonly ApplicationDbContext _context; 
        public ServiceTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ServiceType target, ServiceType source)
        {
            _context.Set<ServiceType>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
