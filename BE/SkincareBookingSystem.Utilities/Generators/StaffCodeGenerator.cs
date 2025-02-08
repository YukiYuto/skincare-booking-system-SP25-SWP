using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Utilities.Generators
{
    public class StaffCodeGenerator
    {
        private static int _counter = 0;

        // Method to generate the code.
        public static string GetStaffCode()
        {
            // Get the current year and extract the last two digits.
            int year = DateTime.Now.Year % 100;

            // Increment the counter for the next call.
            _counter++;

            // Format the code: "S-{last-two-digit-of-year}-xxxx".
            return $"S{year:D2}-{_counter:D4}";
        }

        // Optional: Reset the counter (for testing purposes or if needed).
        public static void ResetCounter()
        {
            _counter = 0;
        }
    }
}
