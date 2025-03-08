using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Booking.Appointment;
using SkincareBookingSystem.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Helpers.Schedules
{
    public static class ScheduleValidator
    {
        /// <summary>
        /// Helper method to check if a therapist is existing against a booking request
        /// </summary>
        /// <param name="therapistSchedule">A TherapistSchedule object containing the therapist's schedule info</param>
        /// <param name="bookingRequest">A DTO containing booking request data</param>
        /// <returns>true if both object's TherapistId, AppointmentDate and SlotId match, false otherwise</returns>
        public static bool IsScheduleExisting(TherapistSchedule therapistSchedule, BookAppointmentDto bookingRequest)
        {
            try
            {
                return (
                therapistSchedule.TherapistId == bookingRequest.TherapistId &&
                therapistSchedule.Appointment.AppointmentDate == bookingRequest.AppointmentDate &&
                therapistSchedule.SlotId == bookingRequest.SlotId
                );
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Helper method to check if a schedule is disabled/ dismissed (Unavailable, Cancelled, Rejected).
        /// The reason for this is to prevent booking / checking on a schedule that is no longer active.
        /// </summary>
        /// <param name="therapistSchedule"></param>
        /// <returns></returns>
        public static bool IsScheduleDisabled(TherapistSchedule therapistSchedule)
        {
            return (
                therapistSchedule.ScheduleStatus == ScheduleStatus.Unavailable ||
                therapistSchedule.ScheduleStatus == ScheduleStatus.Cancelled ||
                therapistSchedule.ScheduleStatus == ScheduleStatus.Rejected
            );
        }
    }
}
