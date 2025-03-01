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

        public static class Customer
        {
            public const string NotFound = "Customer(s) not found";
            public const string Found = "Customer(s) found";
        }

        /// <summary>
        /// Constants related to database operations (e.g. SaveAsync).
        /// </summary>
        public static class Database
        {
            public const int Success = 1;
            public const int Failure = 0;
        }

        public static class File
        {
            public const string FileEmpty = "File is empty";
            public const string FileRetrieved = "File retrieved successfully";
            public const string FileUploaded = "File uploaded successfully";
            public const string FileNotFound = "File not found";
            public const string ImageNotFound = "Image not found";
            public const string ImageUploaded = "Image uploaded successfully";
            public const string VideoUploaded = "Video uploaded successfully";
            public const string VideoNotFound = "Video not found";
        }

        public static class SkinTherapist
        {
            public const string NotFound = "Skin therapist(s) not found";
            public const string Found = "Skin therapist(s) found";
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

        public static class Token
        {
            public const string TokenStored = "Token stored successfully";
            public const string TokenNotFound = "Token not found";
            public const string TokenExpired = "Token expired";
            public const string TokenRefreshed = "Token refreshed successfully";
            public const string TokenInvalid = "Token is invalid";
        }

        /// <summary>
        /// Constants related to user operations.
        /// </summary>
        public static class User {
            public const string UserNotFound = "User not found";
            public const string UserNotAuthorized = "User is not authorized";
        }

        // Constants related to booking schedule operations
        public static class BookingSchedule
        {
            public const string Created = "CREATED";
            public const string Confirmed = "CONFIRMED";
            public const string Cancelled = "CANCELLED";
            public const string Completed = "COMPLETED";
            public const string Rescheduled = "RESCHEDULED";
            public const string Deleted = "DELETED";
        }

    }
}
