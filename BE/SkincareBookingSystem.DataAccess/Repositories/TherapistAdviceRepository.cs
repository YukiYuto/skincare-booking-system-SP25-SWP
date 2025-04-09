using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class TherapistAdviceRepository : Repository<TherapistAdvice>, ITherapistAdviceRepository
{
    private readonly ApplicationDbContext _context;
    public TherapistAdviceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    
}