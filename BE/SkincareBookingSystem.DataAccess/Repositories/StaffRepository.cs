using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class StaffRepository : Repository<Staff>, IStaffRepository
{
    private readonly ApplicationDbContext _context;
    
    public StaffRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}