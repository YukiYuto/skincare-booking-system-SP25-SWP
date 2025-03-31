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
  handleAppointmentClick,
  appointments,
}) => {
  const appointmentsArray = Array.isArray(appointments) ? appointments : [];

  const formatSlotTime = (startTime, endTime) => {
    const formattedStart = moment(startTime, "HH:mm:ss").format("HH:mm");
    const formattedEnd = moment(endTime, "HH:mm:ss").format("HH:mm");
    return `${formattedStart} - ${formattedEnd}`;
  };

  const renderTableHeader = () => {
    const headers = [];
    for (let i = 0; i < 7; i++) {
      const day = moment(currentWeekStart).add(i, "days");
      headers.push(
        <th
          key={day.format("YYYY-MM-DD")}
          className={`${styles.dayHeader} ${moment().isSame(day, "day") ? styles.today : ""}`}
        >
          {day.format("ddd, MMM D")}
        </th>
      );
    }
    return (
      <tr>
        <th className={styles.timeSlotLabel}>Time</th>
        {headers}
      </tr>
    );
  };

  const renderTableRows = () => {
    return slots.map((slot) => {
      const cells = [];
      for (let i = 0; i < 7; i++) {
        const day = moment(currentWeekStart).add(i, "days");
        const appointment = appointmentsArray.find(
          (appointment) =>
            moment(appointment.appointmentDate).isSame(day, "day") &&
            appointment.slotId === slot.slotId
        );

        cells.push(
          <td
            key={`${day.format("YYYY-MM-DD")}-${slot.slotId}`}
            className={`${styles.timeSlotCell} ${appointment ? styles.hasAppointment : ""}`}
            onClick={() => appointment && handleAppointmentClick(appointment.appointmentId)}
          >
            {appointment ? (
              <Tooltip title={`${appointment.status || "Scheduled"}`}>
                <div className={styles.appointmentIndicator}>
                  <Badge color="#1890ff" />
                  <span>{appointment.serviceInfo?.serviceName || "Appointment"}</span>
                </div>
              </Tooltip>
            ) : null}
          </td>
        );
      }
      return (
        <tr key={slot.slotId}>
          <td className={styles.timeSlotLabel}>{formatSlotTime(slot.startTime, slot.endTime)}</td>
          {cells}
        </tr>
      );
    });
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
        <button className={styles.navButton} onClick={handlePreviousWeek}>← Previous Week</button>
        <span className={styles.weekLabel}>
          {currentWeekStart.format("MMM D")} - {moment(currentWeekStart).add(6, "days").format("MMM D, YYYY")}
        </span>
        <button className={styles.navButton} onClick={handleNextWeek}>Next Week →</button>
      </div>

      <table className={styles.slotBasedCalendar}>
        <thead>{renderTableHeader()}</thead>
        <tbody>{renderTableRows()}</tbody>
      </table>
    </div>
  );
};

export default SlotBasedWeekView;
