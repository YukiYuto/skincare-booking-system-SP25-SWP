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
        /// Error messages related to Payment
        /// </summary>
        public static class Payment
        {
            public const string Pending = "Payment is pending";
            public const string Paid = "Payment paid";
            public const string Cancelled = "Payment cancelled";
            public const string NotFound = "Payment not found";
            public const string NotCompleted = "Payment not completed for the specified order";
        }
        /// <summary>
        /// Error messages related to appointments
        /// </summary>
        public static class Appointment
        {
            public const string AlreadyExists = "An appointment has already been booked for this order";
            public const string Created = "Appointment created successfully";
            public const string Deleted = "Appointment deleted successfully";
            public const string Cancelled = "Appointment cancelled successfully";
            public const string RetrievedAll = "All appointments retrieved successfully";
            public const string Retrieved = "Appointment retrieved successfully";
            public const string Updated = "Appointment updated successfully";
            public const string NotFound = "Appointment(s) not found";
            public const string NotCreated = "Failed to create appointment";
            public const string NotDeleted = "Failed to delete appointment";
            public const string NotUpdated = "Failed to update appointment";
            public const string NotRetrieved = "Failed to retrieve appointment(s)";
            public const string NotMatchedToCustomer = "Appointment does not match the customer";
            public const string NotReschedulable = "Appointment cannot be rescheduled";
            public const string NotCancellable = "Appointment cannot be cancelled";
            public const string NotCancelled = "Failed to cancel appointment";
            public const string NotRescheduled = "Failed to reschedule appointment";
            public const string Rescheduled = "Appointment rescheduled successfully";
            public const string RescheduleWithinGracePeriod = "Rescheduling is only allowed up to 24 hours before the appointment";
            public const string CancelWithinGracePeriod = "Cancellation is only allowed up to 24 hours before the appointment";
        }
        /// <summary>
        /// Error messages related to Booking Schedules
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
        /// <summary>
        /// Error messages related to Blog
        /// </summary>
        public static class Blog
        {
            public const string Created = "Blog created successfully";
            public const string Deleted = "Blog deleted successfully";
            public const string AlreadyDeleted = "Blog already deleted";
            public const string RetrievedAll = "All Blog(s) retrieved successfully";
            public const string Retrieved = "Blog retrieved successfully";
            public const string Updated = "Blog updated successfully";
            public const string NotFound = "Blog(s) not found";
            public const string NotCreated = "Failed to create Blog";
            public const string NotDeleted = "Failed to delete Blog";
            public const string NotUpdated = "Failed to update Blog";
            public const string NotRetrieved = "Failed to retrieve Blog(s)";
            public const string NotAuthorized = "You are't allowed to go here";

        }
        /// <summary>
        /// Error messages related to TestQuestion
        /// </summary>
        public static class TestQuestion
        {
            public const string Created = "Test question created successfully";
            public const string Deleted = "Test question deleted successfully";
            public const string AlreadyDeleted = "Test question already deleted";
            public const string RetrievedAll = "All test questions retrieved successfully";
            public const string Retrieved = "Test question retrieved successfully";
            public const string Updated = "Test question updated successfully";
            public const string NotFound = "Test question(s) not found";
            public const string NotCreated = "Failed to create test question";
            public const string NotDeleted = "Failed to delete test question";
            public const string NotUpdated = "Failed to update test question";
            public const string NotRetrieved = "Failed to retrieve test question(s)";
        }
        /// <summary>
        /// Error messages related to TestAnswer
        /// </summary>
        public static class TestAnswer
        {
            public const string Created = "Test answer created successfully";
            public const string Deleted = "Test answer deleted successfully";
            public const string AlreadyDeleted = "Test answer already deleted";
            public const string RetrievedAll = "All test answers retrieved successfully";
            public const string Retrieved = "Test answer retrieved successfully";
            public const string Updated = "Test answer updated successfully";
            public const string NotFound = "Test answer(s) not found";
            public const string NotCreated = "Failed to create test answer";
            public const string NotDeleted = "Failed to delete test answer";
            public const string NotUpdated = "Failed to update test answer";
            public const string NotRetrieved = "Failed to retrieve test answer(s)";
        }

        public static class Customer
        {
            public const string Invalid = "Invalid customer information provided";
            public const string NotFound = "No customer found";
            public const string Found = "Customer found";
            public const string Retrieved = "Customer information retrieved successfully";
        }

        public static class Order
        {
            public const string Created = "Order created successfully";
            public const string Deleted = "Order deleted successfully";
            public const string RetrievedAll = "All orders retrieved successfully";
            public const string Retrieved = "Order retrieved successfully";
            public const string Updated = "Order updated successfully";
            public const string NotFound = "Order(s) not found";
            public const string NotCreated = "Failed to create order";
            public const string NotDeleted = "Failed to delete order";
            public const string NotUpdated = "Failed to update order";
            public const string NotRetrieved = "Failed to retrieve order(s)";
        }

        /// <summary>
        /// Error messages related to Feedback
        /// </summary>
        public static class Feedback
        {
            public const string Created = "Feedback created successfully";
            public const string Deleted = "Feedback deleted successfully";
            public const string AlreadyDeleted = "Feedback already deleted";
            public const string RetrievedAll = "All feedbacks retrieved successfully";
            public const string Retrieved = "Feedback retrieved successfully";
            public const string Updated = "Feedback updated successfully";
            public const string NotFound = "Feedback(s) not found";
            public const string NotCreated = "Failed to create Feedback";
            public const string NotDeleted = "Failed to delete Feedback";
            public const string NotUpdated = "Failed to update Feedback";
            public const string NotRetrieved = "Failed to retrieve Feedback(s)";
        }

        public static class Service
        {
            public const string NotFound = "Service not found";
            public const string NoSimilarServices = "No similar service found";
            public const string SimilarServicesRetrieved = "Similar services retrieved successfully";
        }
        public static class Slot
        {
            public const string InvalidSelected = "Invalid time slot selected";
        }

        public static class TherapistSchedule
        {
            public const string NoTherapistForSlot = "No available therapist for this time slot. Please try another date, or slot.";
            public const string AutoAssignmentHandled = "Therapist auto-assignment handled successfully";
            public const string AlreadyScheduled = "The therapist is already scheduled for this slot";
        }

        public static class BlogCategory
        {
            public const string Created = "Blog category created successfully.";
            public const string NotCreated = "Failed to create blog category.";
            public const string Retrieved = "Blog category retrieved successfully.";
            public const string RetrievedAll = "All blog categories retrieved successfully.";
            public const string NotFound = "Blog category not found.";
            public const string Updated = "Blog category updated successfully.";
            public const string NotUpdated = "Failed to update blog category.";
        }


        public static class SkinTest
        {
            public const string Created = "Skin test created successfully";
            public const string NotCreated = "Failed to create skin test";
            public const string Deleted = "Skin test deleted successfully";
            public const string NotDeleted = "Failed to delete skin test";
            public const string AlreadyDeleted = "Skin test already deleted";
            public const string RetrievedAll = "All skin tests retrieved successfully";
            public const string Retrieved = "Skin test retrieved successfully";
            public const string Updated = "Skin test updated successfully";
            public const string NotUpdated = "Failed to update skin test";
            public const string NotFound = "Skin test(s) not found";
        }

        public static class SkinProfile
        {
            public const string Created = "Skin profile created successfully";
            public const string NotCreated = "Failed to create skin profile";
            public const string Deleted = "Skin profile deleted successfully";
            public const string NotDeleted = "Failed to delete skin profile";
            public const string AlreadyDeleted = "Skin profile already deleted";
            public const string RetrievedAll = "All skin profiles retrieved successfully";
            public const string Retrieved = "Skin profile retrieved successfully";
            public const string Updated = "Skin profile updated successfully";
            public const string NotUpdated = "Failed to update skin profile";
            public const string NotFound = "Skin profile(s) not found";
        }
    }
}
