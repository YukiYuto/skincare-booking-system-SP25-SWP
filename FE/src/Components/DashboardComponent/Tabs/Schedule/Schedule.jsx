import React, { useEffect, useState } from "react";
import { Modal, Spin, Tooltip, message } from "antd";
import styles from "./Schedule.module.css";
import { apiCall } from "../../../../utils/apiUtils";
import {
  GET_ALL_SLOTS_API,
  GET_ALL_THERAPISTS_API,
  GET_THERAPIST_SCHEDULE_BY_THERAPIST_API,
  GET_APPOINTMENT_BY_ID_API,
} from "../../../../config/apiConfig";
import AppointmentDetail from "../../../Appointment/AppointmentDetail/AppointmentDetail";
import moment from "moment";

const Schedule = () => {
  const [therapists, setTherapists] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [therapistSchedule, setTherapistSchedule] = useState(null);
  const [selectedAppointment, setSelectedAppointment] = useState(null);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [currentWeekStart, setCurrentWeekStart] = useState(
    moment().startOf("week")
  );
  const [slots, setSlots] = useState([]);

  const fetchData = async (api, setter, errorMsg) => {
    try {
      const data = await apiCall("GET", api);
      if (data.isSuccess) setter(data.result);
      else throw new Error(data.message || errorMsg);
    } catch (err) {
      message.error(err.message || errorMsg);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData(
      GET_ALL_THERAPISTS_API,
      setTherapists,
      "Failed to fetch therapists"
    );
    fetchData(GET_ALL_SLOTS_API, setSlots, "Failed to fetch slots");
  }, []);

  const handleTherapistChange = async (event) => {
    const therapistId = event.target.value;
    if (!therapistId) return setTherapistSchedule(null);

    fetchData(
      GET_THERAPIST_SCHEDULE_BY_THERAPIST_API.replace(
        "{therapistId}",
        therapistId
      ),
      setTherapistSchedule,
      "Failed to fetch therapist schedule"
    );
  };

  const handleCardClick = async (appointmentId) => {
    fetchData(
      GET_APPOINTMENT_BY_ID_API.replace("{appointmentId}", appointmentId),
      setSelectedAppointment,
      "Failed to fetch appointment details"
    );
    setIsModalVisible(true);
  };

  const renderTableRows = () =>
    slots.map((slot) => (
      <tr key={slot.slotId}>
        <td>
          {moment(slot.startTime, "HH:mm:ss").format("HH:mm")} -{" "}
          {moment(slot.endTime, "HH:mm:ss").format("HH:mm")}
        </td>
        {[...Array(7)].map((_, i) => {
          const day = moment(currentWeekStart).add(i, "days");
          const appointment = therapistSchedule?.find(
            (s) =>
              moment(s.appointmentDate).format("YYYY-MM-DD") ===
                day.format("YYYY-MM-DD") && s.slotId === slot.slotId
          );
          return (
            <td
              key={`${day.format("YYYY-MM-DD")}-${slot.slotId}`}
              className={appointment ? styles.hasAppointment : ""}
              onClick={() =>
                appointment && handleCardClick(appointment.appointmentId)
              }
            >
              {appointment && (
                <Tooltip
                  title={`${
                    appointment.status || "Scheduled"
                  } - Click for details`}
                >
                  <div>
                    {appointment.serviceInfo?.serviceName || "Appointment"}
                  </div>
                </Tooltip>
              )}
            </td>
          );
        })}
      </tr>
    ));

  return (
    <div className={styles.tabContainer}>
      <h2 className={styles.tabTitle}>Schedule</h2>
      {loading ? (
        <div className={styles.loadingContainer}>
          <Spin size="large" />
          <p>Loading...</p>
        </div>
      ) : error ? (
        <div className={styles.errorContainer}>
          <p>Error: {error}</p>
          <button
            onClick={() => window.location.reload()}
            className={styles.retryButton}
          >
            Retry
          </button>
        </div>
      ) : (
        <>
          <select
            onChange={handleTherapistChange}
            className={styles.therapistSelect}
          >
            <option value="">Select a therapist</option>
            {therapists.map(({ skinTherapistId, fullName }) => (
              <option key={skinTherapistId} value={skinTherapistId}>
                {fullName}
              </option>
            ))}
          </select>
          {therapistSchedule && (
            <div className={styles.slotBasedWeekContainer}>
              <div className={styles.weekNavigation}>
                <button
                  className={styles.navButton}
                  onClick={() =>
                    setCurrentWeekStart(
                      moment(currentWeekStart).subtract(1, "week")
                    )
                  }
                >
                  ← Previous
                </button>
                <span className={styles.weekLabel}>
                  {currentWeekStart.format("MMM D")} -{" "}
                  {moment(currentWeekStart)
                    .add(6, "days")
                    .format("MMM D, YYYY")}
                </span>
                <button
                  className={styles.navButton}
                  onClick={() =>
                    setCurrentWeekStart(moment(currentWeekStart).add(1, "week"))
                  }
                >
                  Next →
                </button>
              </div>
              <table className={styles.scheduleTable}>
                <thead>
                  <tr>
                    <th>Time</th>
                    {[...Array(7)].map((_, i) => {
                      const day = moment(currentWeekStart).add(i, "days");
                      const isCurrentDay = day.isSame(moment(), "day");
                      return (
                        <th
                          key={i}
                          className={isCurrentDay ? styles.currentDay : ""}
                        >
                          {day.format("ddd, MMM D")}
                        </th>
                      );
                    })}
                  </tr>
                </thead>
                <tbody>{renderTableRows()}</tbody>
              </table>
            </div>
          )}
        </>
      )}
      <Modal
        title="Appointment Details"
        visible={isModalVisible}
        onCancel={() => setIsModalVisible(false)}
        footer={null}
      >
        {selectedAppointment ? (
          <AppointmentDetail appointment={selectedAppointment} />
        ) : (
          <p>Loading...</p>
        )}
      </Modal>
    </div>
  );
};

export default Schedule;
