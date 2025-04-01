using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.DataAccess.Seed;

public class ApplicationDbContextSeed
{
    public static void SeedAdminAndManagerAccount(ModelBuilder modelBuilder)
    {
        var managerRoleId = "8fa7c7bb-b4dd-480d-a660-e07a90855d5s";
        var adminRoleId = "8fa7c7bb-daa5-a660-bf02-82301a5eb32a";

        var adminUserId = "SkinBookingSystem-Admin";
        var managerUserId = "SkinBookingSystem-Manager";

        var roles = new List<IdentityRole>
        {
            new()
            {
                Id = managerRoleId,
                ConcurrencyStamp = StaticUserRoles.Manager,
                Name = StaticUserRoles.Manager,
                NormalizedName = StaticUserRoles.Manager.ToUpper()
            },
            new()
            {
                Id = adminRoleId,
                ConcurrencyStamp = StaticUserRoles.Admin,
                Name = StaticUserRoles.Admin,
                NormalizedName = StaticUserRoles.Admin.ToUpper()
            }
        };

        modelBuilder.Entity<IdentityRole>().HasData(roles);

        var hasher = new PasswordHasher<ApplicationUser>();

        var adminUser = new ApplicationUser
        {
            Id = adminUserId,
            FullName = "Admin",
            Age = 30,
            ImageUrl = "https://example.com/avatar.png",
            Address = "123 Admin St",
            UserName = "admin@gmail.com",
            NormalizedUserName = "ADMIN@GMAIL.COM",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(null, "Admin123!"),
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            PhoneNumber = "1234567890",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = true,
            AccessFailedCount = 0
        };

        var managerUser = new ApplicationUser
        {
            Id = managerUserId,
            FullName = "Manager",
            Age = 30,
            ImageUrl = "https://example.com/avatarManager.png",
            Address = "123 Manager St",
            UserName = "manager@gmail.com",
            NormalizedUserName = "MANAGER@GMAIL.COM",
            Email = "manager@gmail.com",
            NormalizedEmail = "MANAGER@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(null, "Manager123!"),
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            PhoneNumber = "0123456789",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = true,
            AccessFailedCount = 0
        };

        modelBuilder.Entity<ApplicationUser>().HasData(adminUser, managerUser);

        // Assigning the admin role to the admin user (ĐÚNG CÁCH)
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = adminRoleId,
            UserId = adminUserId
        });

        // Assign the Manager role to the manager users (ĐÚNG CÁCH)
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = managerRoleId,
            UserId = managerUserId
        });
    }
}