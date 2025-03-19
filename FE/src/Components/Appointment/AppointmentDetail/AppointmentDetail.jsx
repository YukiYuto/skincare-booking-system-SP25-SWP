import React from "react";
import styles from "./AppointmentDetail.module.css";

const AppointmentDetail = ({ appointment }) => {
  if (!appointment) return null;

  const {
    customerInfo,
    serviceInfo,
    therapistInfo,
    appointmentDate,
    appointmentTime,
    createdTime,
    note,
    status,
  } = appointment;

  return (
    <div className={styles.appointmentDetail}>
      <h3>Appointment Details</h3>

      <div className={styles.detailSection}>
        <h4>Customer Information</h4>
        <p>
          <strong>Name:</strong> {customerInfo.customerName}
        </p>
        <p>
          <strong>Phone:</strong> {customerInfo.customerPhone}
        </p>
        <p>
          <strong>Email:</strong> {customerInfo.customerEmail}
        </p>
      </div>

      <div className={styles.detailSection}>
        <h4>Service Information</h4>
        <p>
          <strong>Service:</strong> {serviceInfo.serviceName}
        </p>
        <p>
          <strong>Price:</strong> {serviceInfo.servicePrice}
        </p>
        {serviceInfo.serviceDuration && (
          <p>
            <strong>Duration:</strong> {serviceInfo.serviceDuration}
          </p>
        )}
      </div>

      <div className={styles.detailSection}>
        <h4>Therapist Information</h4>
        <p>
          <strong>Name:</strong> {therapistInfo.therapistName}
        </p>
        <p>
          <strong>Age:</strong> {therapistInfo.therapistAge}
        </p>
      </div>

      <div className={styles.detailSection}>
        <h4>Appointment Information</h4>
        <p>
          <strong>Date:</strong> {appointmentDate}
        </p>
        <p>
          <strong>Time:</strong> {appointmentTime}
        </p>
        <p>
          <strong>Created:</strong> {createdTime}
        </p>
        <p>
          <strong>Status:</strong>{" "}
          <span className={styles.status}>{status}</span>
        </p>
        {note && (
          <p>
            <strong>Note:</strong> {note}
          </p>
        )}
      </div>
    </div>
  );
};

export default AppointmentDetail;
