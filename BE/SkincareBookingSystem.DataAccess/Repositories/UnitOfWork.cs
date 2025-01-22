using Microsoft.AspNetCore.Identity;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IUserManagerRepository UserManagerRepository { get; private set; }

    public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        UserManagerRepository = new UserManagerRepository(userManager);
        
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}