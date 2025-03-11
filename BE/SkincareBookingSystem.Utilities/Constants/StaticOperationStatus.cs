using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Utilities.Constants
{
    public static class StaticOperationStatus
    {
        public static class Appointment
        {
            public const string Created = "CREATED";
            public const string Confirmed = "CONFIRMED";
            public const string Cancelled = "CANCELLED";
            public const string Completed = "COMPLETED";
            public const string Rescheduled = "RESCHEDULED";
            public const string Deleted = "DELETED";
        }

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

        public static class Order
        {
            public static class Message
            {
                public const string Created = "Order created successfully";
                public const string NotCreated = "Failed to create order";
                public const string NotFound = "Order not found";
                public const string Found = "Order found";
                public const string EmptyList = "Order list is empty";
                public const string Invalid = "Invalid order";
            }

            public const string Created = "CREATED";
            public const string Paid = "PAID";
            public const string Cancelled = "CANCELLED";
        }

        public static class OrderDetail
        {
            public const string NotFound = "No order details found";
            public const string Found = "Order details found";
            public const string EmptyList = "Order detail list is empty";
            public const string NotProvided = "No order details provided";
            public const string Invalid = "Invalid order details";
            public const string Created = "Order detail(s) created successfully";
            public const string NotCreated = "Failed to create order detail(s)";
        }

        public static class ServiceType
        {
            public const string NotFound = "Service type(s) not found";
            public const string Found = "Service type(s) found";
            public const string EmptyList = "Service type list is empty";
            public const string Invalid = "Invalid service type(s)";
        }
        
        public static class Service 
        {
            public const string Active = "Active";
            public const string Disable = "Disable";
        }

        public static class SkinTherapist
        {
            public const string NotFound = "Skin therapist(s) not found";
            public const string Found = "Skin therapist(s) found";
        }

        public static class Slot
        {
            public const string NotFound = "No slot found";
            public const string Found = "Slot found";
            public const string EmptyList = "Slot list is empty";
            public const string Invalid = "Invalid slot";
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

        public static class TherapistServiceType
        {
            public const string Added = "Service type(s) added to therapist";
            public const string Removed = "Service type(s) removed from therapist";
            public const string NotFound = "No record found";
            public const string NotAdded = "Failed to add service type(s) to therapist";
            public const string NotRemoved = "Failed to remove service type(s) from therapist";
        }

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

        public static class User
        {
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

        // Constants related to Blog
        public static class Blog
        {
            public const string Drafting = "DRAFTING...";   // Blog đang trong giai đoạn thai nghén, chưa công khai
            public const string Pending_review = "PENDING...";  // Blog đang chờ kiểm duyệt
            public const string Approved = "APPROVED";  // Blog đã được kiểm duyệt và sẵn sàng công khai
            public const string Published = "PUBLISHED"; // Blog đã được lưu lại và công khai
            public const string Archieved = "ARCHIEVED"; // Blog đã được lưu lại nhưng không công khai
            public const string Deleted = "DELETED";    // Xóa Blog
        }

        // Constants related to BlogCategory
        public static class BlogCategory
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
