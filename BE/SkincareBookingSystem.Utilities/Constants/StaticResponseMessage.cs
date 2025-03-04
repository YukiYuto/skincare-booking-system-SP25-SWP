using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Utilities.Constants
{
    /// <summary>
    /// ErrorMessage class, contains all error messages that can be returned in ResponseDto
    /// </summary>
    public static class StaticResponseMessage
    {
        // Default error message
        public const string DefaultError = "An error has occurred";
        public const string Success = "Operation successful";
        /// <summary>
        /// Error messages related to users
        /// </summary>
        public static class User
        {
            public const string Created = "User created successfully";
            public const string Deleted = "User deleted successfully";
            public const string RetrievedAll = "All users retrieved successfully";
            public const string Retrieved = "User retrieved successfully";
            public const string Updated = "User updated successfully";
            public const string NotFound = "User not found";
            public const string NotCreated = "Failed to create user";
            public const string NotDeleted = "Failed to delete user";
            public const string NotUpdated = "Failed to update user";
            public const string NotRetrieved = "Failed to retrieve user(s)";
        }
        /// <summary>
        /// Error messages related to appointments
        /// </summary>
        public static class Appointment
        {
            public const string Created = "Appointment created successfully";
            public const string Deleted = "Appointment deleted successfully";
            public const string RetrievedAll = "All appointments retrieved successfully";
            public const string Retrieved = "Appointment retrieved successfully";
            public const string Updated = "Appointment updated successfully";
            public const string NotFound = "Appointment(s) not found";
            public const string NotCreated = "Failed to create appointment";
            public const string NotDeleted = "Failed to delete appointment";
            public const string NotUpdated = "Failed to update appointment";
            public const string NotRetrieved = "Failed to retrieve appointment(s)";
        }
        /// <summary>
        /// Error messages related to booking schedules
        /// </summary>
        public static class BookingSchedule
        {
            public const string Created = "Booking schedule created successfully";
            public const string Deleted = "Booking schedule deleted successfully";
            public const string AlreadyDeleted = "Booking schedule already deleted";
            public const string RetrievedAll = "All booking schedules retrieved successfully";
            public const string Retrieved = "Booking schedule retrieved successfully";
            public const string Updated = "Booking schedule updated successfully";
            public const string NotFound = "Booking schedule(s) not found";
            public const string NotCreated = "Failed to create booking schedule";
            public const string NotDeleted = "Failed to delete booking schedule";
            public const string NotUpdated = "Failed to update booking schedule";
            public const string NotRetrieved = "Failed to retrieve booking schedule(s)";
        }
    }
}
