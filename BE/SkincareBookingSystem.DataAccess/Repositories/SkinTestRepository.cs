using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class SkinTestRepository : Repository<SkinTest>, ISkinTestRepository
    {
        private readonly ApplicationDbContext _context; 
        public SkinTestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(SkinTest target, SkinTest source)
        {
            _context.Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
