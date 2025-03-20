import React from "react";
import { Spin, Tooltip, Badge } from "antd";
import moment from "moment";
import styles from "./SlotBasedWeekView.module.css";

const SlotBasedWeekView = ({
  slots,
  slotsLoading,
  currentWeekStart,
  handlePreviousWeek,
  handleNextWeek,
  handleDateSelect,
  handleAppointmentClick,
  appointments,
}) => {
  // Ensure appointments is always an array
  const appointmentsArray = Array.isArray(appointments) ? appointments : [];

  // Helper function to normalize date format
  const formatDateToYYYYMMDD = (dateString) => {
    if (!dateString) return "";
    
    // Check if date is already in YYYY-MM-DD format
    if (/^\d{4}-\d{2}-\d{2}$/.test(dateString)) {
      return dateString;
    }
    
    // Handle M/D/YYYY format
    if (dateString.includes('/')) {
      const parts = dateString.split('/');
      if (parts.length === 3) {
        const month = parts[0].padStart(2, '0');
        const day = parts[1].padStart(2, '0');
        const year = parts[2];
        return `${year}-${month}-${day}`;
      }
    }
    
    // Fallback - try to parse with moment
    return moment(dateString).format("YYYY-MM-DD");
  };

  const isAppointmentInSlot = (day, slot) => {
    const dayFormatted = day.format("YYYY-MM-DD");
    const slotStart = moment(slot.startTime, "HH:mm:ss");
    const slotEnd = moment(slot.endTime, "HH:mm:ss");

    return appointmentsArray.some((appointment) => {
      // Skip if appointment is invalid
      if (!appointment) {
        return false;
      }

      // Format appointment date consistently
      const appointmentDate = formatDateToYYYYMMDD(appointment.appointmentDate);
      
      // Check date match - removing debug logs
      if (appointmentDate !== dayFormatted) {
        return false;
      }

      // Check if appointmentTime exists and is a string
      if (!appointment.appointmentTime || typeof appointment.appointmentTime !== 'string') {
        return false;
      }

      // Split time range - handle both formats: "HH:mm:ss - HH:mm:ss" and "HH:mm - HH:mm"
      const appointmentTimeParts = appointment.appointmentTime.split(" - ");
      if (appointmentTimeParts.length !== 2) {
        return false;
      }

      // Parse appointment times - try multiple formats
      let appointmentStart, appointmentEnd;
      
      try {
        appointmentStart = moment(appointmentTimeParts[0], ["HH:mm:ss", "HH:mm"]);
        appointmentEnd = moment(appointmentTimeParts[1], ["HH:mm:ss", "HH:mm"]);
        
        if (!appointmentStart.isValid() || !appointmentEnd.isValid()) {
          return false;
        }
      } catch (error) {
        console.error("Error parsing appointment time:", error);
        return false;
      }

      // Check for overlap
      return (
        (appointmentStart.isSameOrAfter(slotStart) &&
          appointmentStart.isBefore(slotEnd)) ||
        (appointmentEnd.isAfter(slotStart) &&
          appointmentEnd.isSameOrBefore(slotEnd)) ||
        (appointmentStart.isSameOrBefore(slotStart) &&
          appointmentEnd.isSameOrAfter(slotEnd))
      );
    });
  };

  const getAppointmentForSlot = (day, slot) => {
    const dayFormatted = day.format("YYYY-MM-DD");
    const slotStart = moment(slot.startTime, "HH:mm:ss");
    const slotEnd = moment(slot.endTime, "HH:mm:ss");

    return appointmentsArray.find((appointment) => {
      // Skip if appointment is invalid
      if (!appointment) {
        return false;
      }

      // Format appointment date consistently
      const appointmentDate = formatDateToYYYYMMDD(appointment.appointmentDate);
      
      // Check date match
      if (appointmentDate !== dayFormatted) {
        return false;
      }

      // Check if appointmentTime exists and is a string
      if (!appointment.appointmentTime || typeof appointment.appointmentTime !== 'string') {
        return false;
      }

      // Split time range
      const appointmentTimeParts = appointment.appointmentTime.split(" - ");
      if (appointmentTimeParts.length !== 2) {
        return false;
      }

      // Parse appointment times - try multiple formats
      let appointmentStart, appointmentEnd;
      
      try {
        appointmentStart = moment(appointmentTimeParts[0], ["HH:mm:ss", "HH:mm"]);
        appointmentEnd = moment(appointmentTimeParts[1], ["HH:mm:ss", "HH:mm"]);
        
        if (!appointmentStart.isValid() || !appointmentEnd.isValid()) {
          return false;
        }
      } catch (error) {
        console.error("Error parsing appointment time:", error);
        return false;
      }

      // Check for overlap
      return (
        (appointmentStart.isSameOrAfter(slotStart) &&
          appointmentStart.isBefore(slotEnd)) ||
        (appointmentEnd.isAfter(slotStart) &&
          appointmentEnd.isSameOrBefore(slotEnd)) ||
        (appointmentStart.isSameOrBefore(slotStart) &&
          appointmentEnd.isSameOrAfter(slotEnd))
      );
    });
  };

  const formatSlotTime = (startTime, endTime) => {
    const formattedStart = moment(startTime, "HH:mm:ss").format("HH:mm");
    const formattedEnd = moment(endTime, "HH:mm:ss").format("HH:mm");
    return `${formattedStart} - ${formattedEnd}`;
  };

  const renderSlotRow = (slot, dayIndex) => {
    const days = [];

    for (let i = 0; i < 7; i++) {
      const day = moment(currentWeekStart).add(i, "days");
      const hasAppointment = isAppointmentInSlot(day, slot);
      const appointment = hasAppointment
        ? getAppointmentForSlot(day, slot)
        : null;

      days.push(
        <div
          key={`${day.format("YYYY-MM-DD")}-${slot.slotId}`}
          className={`${styles.timeSlotCell} ${
            hasAppointment ? styles.hasAppointment : ""
          }`}
          onClick={() => handleDateSelect(day)}
        >
          {hasAppointment && appointment && (
            <Tooltip title={`${appointment.status || 'Scheduled'} - Click for details`}>
              <div
                className={styles.appointmentIndicator}
                onClick={(e) => {
                  e.stopPropagation();
                  if (appointment.appointmentId) {
                    handleAppointmentClick(appointment.appointmentId);
                  }
                }}
              >
                <Badge color="#1890ff" />
                <span>
                  {appointment.serviceInfo?.serviceName || "Appointment"}
                </span>
              </div>
            </Tooltip>
          )}
        </div>
      );
    }

    return (
      <div key={slot.slotId} className={styles.timeSlotRow}>
        <div className={styles.timeSlotLabel}>
          {formatSlotTime(slot.startTime, slot.endTime)}
        </div>
        <div className={styles.timeSlotCells}>{days}</div>
      </div>
    );
  };

  const renderDayHeader = () => {
    const days = [];
    for (let i = 0; i < 7; i++) {
      const day = moment(currentWeekStart).add(i, "days");
      const isToday =
        moment().format("YYYY-MM-DD") === day.format("YYYY-MM-DD");

      days.push(
        <div
          key={day.format("YYYY-MM-DD")}
          className={`${styles.dayHeader} ${isToday ? styles.today : ""}`}
          onClick={() => handleDateSelect(day)}
        >
          <div className={styles.dayName}>{day.format("ddd")}</div>
          <div className={styles.dayDate}>{day.format("MMM D")}</div>
        </div>
      );
    }

    return (
      <div className={styles.dayHeaderRow}>
        <div className={styles.timeSlotLabel}>Time</div>
        <div className={styles.dayHeaders}>{days}</div>
      </div>
    );
  };

  if (slotsLoading) {
    return (
      <div className={styles.spinnerContainer}>
        <Spin size="large" />
        <p>Loading time slots...</p>
      </div>
    );
  }

  return (
    <div className={styles.slotBasedWeekContainer}>
      <div className={styles.weekNavigation}>
        <button className={styles.navButton} onClick={handlePreviousWeek}>
          ← Previous Week
        </button>
        <span className={styles.weekLabel}>
          {currentWeekStart.format("MMM D")} -{" "}
          {moment(currentWeekStart).add(6, "days").format("MMM D, YYYY")}
        </span>
        <button className={styles.navButton} onClick={handleNextWeek}>
          Next Week →
        </button>
      </div>

      <div className={styles.slotBasedCalendar}>
        {renderDayHeader()}
        <div className={styles.timeSlotRows}>
          {slots.map((slot, index) => renderSlotRow(slot, index))}
        </div>
      </div>
    </div>
  );
};

export default SlotBasedWeekView;