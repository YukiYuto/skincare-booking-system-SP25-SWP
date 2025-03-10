﻿using System;
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

        public static class Customer
        {
            public const string Invalid = "Invalid customer information provided";
            public const string NotFound = "No customer found";
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

        public static class Payment
        {
            public const string Pending = "Payment is pending";
            public const string Paid = "Payment paid";
            public const string Cancelled = "Payment cancelled";
            public const string NotFound = "Payment not found";
            public const string NotCompleted = "Payment not completed for the specified order";
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
    }
}
