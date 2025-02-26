using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class SlotRepository : Repository<Slot>, ISlotRepository
    {
        private readonly ApplicationDbContext _context; 
        public SlotRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update (Slot target, Slot source)
        {
            _context.Set<Slot>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
