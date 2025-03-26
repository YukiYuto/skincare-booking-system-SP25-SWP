using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Helpers.Schedules
{
    public static class DateValidator
    {
        public static bool IsDateWithinRange(DateOnly dateToCheck, DateOnly startDate, DateOnly endDate)
        {
            return dateToCheck >= startDate && dateToCheck <= endDate;
        }
    }
}
