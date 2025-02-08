using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Utilities.Generators
{
    public static class StaffCodeGenerator
    {
        // Method to generate the code.
        public static string GetStaffCode()
        {
            // Get the current year and extract the last two digits.
            int year = DateTime.Now.Year % 100;

            // Increment the counter for the next call.
            
            // Format the code: "S-{last-two-digit-of-year}-xxxx".
            return $"S{year:D2}-{GetRandomCounter():D4}";
        }

        private static int GetRandomCounter()
        {
            var random = new Random();
            return random.Next(1, 9999);
        }
    }
}
