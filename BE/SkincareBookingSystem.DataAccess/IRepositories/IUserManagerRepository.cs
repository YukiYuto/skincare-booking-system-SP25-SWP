using Microsoft.AspNetCore.Identity;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IUserManagerRepository
{
    Task<ApplicationUser> FindByEmailAsync(string email);
    Task<bool> CheckIfPhoneNumberExistsAsync(string phoneNumber);
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    Task<ApplicationUser> FindByIdAsync(string userId);
    Task<ApplicationUser> FindByPhoneAsync(string phoneNumber);
    Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string role);
}