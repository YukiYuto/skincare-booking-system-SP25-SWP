import React, { useEffect, useState } from "react";
import styles from "./CustomerAppointment.module.css";
import { GET_ALL_APPOINTMENTS_API } from "../../../../../config/apiConfig";
import { apiCall } from "../../../../../utils/apiUtils";

const CustomerAppointment = ({ customerId }) => {
  const [appointments, setAppointments] = useState([]);
  useEffect(() => {
    const fetchAppointments = async () => {
      try {
        console.log("GET_ALL_APPOINTMENTS_API:", GET_ALL_APPOINTMENTS_API);
        const res = await apiCall("GET", GET_ALL_APPOINTMENTS_API);
        if (res?.isSuccess) {
          const filtered = res.result.filter(
            (appt) => appt.customerId === customerId
          );
          setAppointments(filtered);
        }
      } catch (err) {
        console.error("Failed to fetch appointments:", err.message || err);
      }
    };
    fetchAppointments();
  }, [customerId]);
  return (
    <div className={styles.appointmentList}>
      {appointments.length === 0 ? (
        <p>No appointments found.</p>
      ) : (
        appointments.map((appt) => (
          <div key={appt.appointmentId} className={styles.appointmentItem}>
            <p>
              <strong>Date:</strong> {appt.appointmentDate}
            </p>
            <p>
              <strong>Time:</strong> {appt.appointmentTime}
            </p>
            <p>
              <strong>Note:</strong> {appt.note ? appt.note : "none"}
            </p>
            <p>
              <strong>Status:</strong> {appt.status}
            </p>
          </div>
        ))
      )}
    </div>
  );
};
export default CustomerAppointment;
