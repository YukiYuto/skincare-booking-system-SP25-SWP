using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Helpers.Users
{
    public static class UserError
    {
        /// <summary>
        /// Helper method to check if a user exists
        /// </summary>
        /// <param name="User">User, the ClaimsPrincipal object passed into the service methods</param>
        /// <returns>true if the user doesn't exist or not authorized, false otherwise</returns>
        public static bool NotExists(ClaimsPrincipal User)
        {
            return User is null || (
                User.FindFirstValue(ClaimTypes.NameIdentifier) is null &&
                User.FindFirstValue("FullName") is null
                );
        }
    }
}
