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
                therapistSchedule.ScheduleStatus == ScheduleStatus.Rejected ||
                therapistSchedule.ScheduleStatus == ScheduleStatus.Rescheduled
            );
        }
        /// <summary>
        /// Helper method to check if a schedule is reschedulable.
        /// </summary>
        /// <param name="therapistSchedules">A Collection of TherapistSchedule to be validated</param>
        /// <returns>true if there is no schedule completed / cancelled, false otherwise</returns>
        public static bool IsScheduleReschedulable(ICollection<TherapistSchedule> therapistSchedules)
        {
            return therapistSchedules.Any(
                    ts => ts.ScheduleStatus != ScheduleStatus.Completed &&
                    ts.ScheduleStatus != ScheduleStatus.Cancelled);
        }
        /// <summary>
        /// Helper method to check if an appointment is within the grace period (24 hours).
        /// If the appointment is within the grace period, it cannot be rescheduled or cancelled.
        /// </summary>
        /// <param name="appointment">The appointment to be checked on rescheduling/cancellation request</param>
        /// <returns>true if the appointment's due time is within 24 hours, false otherwise</returns>
        public static bool IsWithinGracePeriod(Appointments appointment)
        {
            var latestSchedule = appointment.TherapistSchedules
                .Where(ts => ts.ScheduleStatus != ScheduleStatus.Rescheduled &&
                             ts.ScheduleStatus != ScheduleStatus.Cancelled)
                .OrderByDescending(ts => ts.CreatedTime)
                .FirstOrDefault();

            if (latestSchedule is null)
                return false;  // No schedule found, return false


            var appointmentDueTime = Convert.ToDateTime(appointment.AppointmentDate + " " + latestSchedule.Slot.StartTime);
            var utcNowVietnam = StaticOperationStatus.Timezone.Vietnam;

            return (appointmentDueTime - utcNowVietnam) < TimeSpan.FromHours(24);
        }
    }
}
