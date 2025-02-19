using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Utilities.Constants
{
    public static class StaticOperationStatus
    {
        /// <summary>
        /// Constants related to appointment statuses.
        /// </summary>
        public static class Appointment
        {
            public const string Created = "CREATED";
            public const string Confirmed = "CONFIRMED";
            public const string Cancelled = "CANCELLED";
            public const string Completed = "COMPLETED";
            public const string Rescheduled = "RESCHEDULED";
            public const string Deleted = "DELETED";
        }
        /// <summary>
        /// Constants related to BaseEntity statuses.
        /// </summary>
        public static class BaseEntity
        {
            public const string Active = "1";
            public const string Deleted = "0";
        }
        /// <summary>
        /// Constants related to database operations (e.g. SaveAsync).
        /// </summary>
        public static class Database
        {
            public const int Success = 1;
            public const int Failure = 0;
        }

        public static class StatusCode
        {
            public const int Ok = 200;
            public const int Created = 201;
            public const int NoContent = 204;
            public const int BadRequest = 400;
            public const int Unauthorized = 401;
            public const int Forbidden = 403;
            public const int NotFound = 404;
            public const int InternalServerError = 500;
        }


        /// <summary>
        /// Constants related to timezones (primarily Vietnam).
        /// </summary>
        public static class Timezone
        {
            public static readonly DateTime Vietnam = DateTime.UtcNow.AddHours(7.0);
        }
    }
}
