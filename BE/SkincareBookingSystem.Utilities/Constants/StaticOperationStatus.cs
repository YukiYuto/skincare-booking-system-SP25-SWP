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
    }
}
