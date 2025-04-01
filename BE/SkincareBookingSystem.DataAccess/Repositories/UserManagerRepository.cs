using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class UserManagerRepository : IUserManagerRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserManagerRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CheckIfPhoneNumberExistsAsync(string phoneNumber)
    {
        return await _userManager.Users.AnyAsync(a =>a.PhoneNumber == phoneNumber);
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
    {
        return await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<ApplicationUser> FindByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<ApplicationUser> FindByPhoneAsync(string phoneNumber)
    {
        return await _userManager.Users.FirstOrDefaultAsync(a => a.PhoneNumber == phoneNumber);
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string role)
    {
        return await _userManager.GetUsersInRoleAsync(role);
    }
}