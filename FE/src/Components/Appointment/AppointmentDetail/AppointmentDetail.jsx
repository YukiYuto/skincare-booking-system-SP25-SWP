import React from "react";
import styles from "./AppointmentDetail.module.css";
import moment from "moment";
import FeedbackContainer from "../../Feedback/FeedbackContainer";

const AppointmentDetail = ({ appointment }) => {
  if (!appointment) return null;

  const {
    appointmentId,
    customerInfo = {},
    serviceInfo = {},
    therapistInfo = {},
    appointmentDate,
    appointmentTime,
    createdTime,
    note,
    status,
  } = appointment;

  const formatDate = (dateString) => {
    if (!dateString) return "N/A";
    try {
      if (/^\d{4}-\d{2}-\d{2}$/.test(dateString)) {
        return moment(dateString, "YYYY-MM-DD").format("MMMM D, YYYY");
      } else if (dateString.includes("/")) {
        const parts = dateString.split("/");
        if (parts.length === 3) {
          return moment(
            `${parts[2]}-${parts[0].padStart(2, "0")}-${parts[1].padStart(
              2,
              "0"
            )}`,
            "YYYY-MM-DD"
          ).format("MMMM D, YYYY");
        }
      }
      return moment(dateString, moment.ISO_8601, true).format("MMMM D, YYYY");
    } catch (e) {
      console.error("Error formatting date:", e);
      return dateString;
    }
  };

  const getStatusClass = () => {
    if (!status) return "";
    return `status ${status.toUpperCase()}`;
  };

  const defaultFeedbackTitle = therapistInfo.therapistName
    ? `Feedback for ${therapistInfo.therapistName}`
    : "Feedback for Appointment";

  return (
    <div className={styles.appointmentDetail}>
      <div className={styles.gridContainer}>
        <div className={styles.detailSection}>
          <h4>Customer Information</h4>
          <p>
            <strong>Name:</strong> {customerInfo.customerName || "N/A"}
          </p>
          <p>
            <strong>Phone:</strong> {customerInfo.customerPhone || "N/A"}
          </p>
          <p>
            <strong>Email:</strong> {customerInfo.customerEmail || "N/A"}
          </p>
        </div>

        <div className={styles.detailSection}>
          <h4>Appointment Information</h4>
          <p>
            <strong>Date:</strong> {formatDate(appointmentDate)}
          </p>
          <p>
            <strong>Time:</strong> {appointmentTime || "N/A"}
          </p>
          <p>
            <strong>Created:</strong> {createdTime || "N/A"}
          </p>
          <p>
            <strong>Status:</strong>{" "}
            <span className={styles[getStatusClass()]}>{status || "N/A"}</span>
          </p>
          {note && (
            <p>
              <strong>Note:</strong> {note}
            </p>
          )}
        </div>

        <div className={styles.detailSection2}>
          <h4>Therapist Information</h4>
          <p>
            <strong>Name:</strong> {therapistInfo.therapistName || "N/A"}
          </p>
          <p>
            <strong>Age:</strong> {therapistInfo.therapistAge || "N/A"}
          </p>
        </div>

        <div className={styles.detailSection2}>
          <h4>Service Information</h4>
          <p>
            <strong>Service:</strong> {serviceInfo.serviceName || "N/A"}
          </p>
          <p>
            <strong>Price:</strong>{" "}
            {serviceInfo.servicePrice
              ? `${serviceInfo.servicePrice.toLocaleString()}₫`
              : "N/A"}
          </p>
          {serviceInfo.serviceDuration && (
            <p>
              <strong>Duration:</strong> {serviceInfo.serviceDuration}
            </p>
          )}
        </div>

        <div className={styles.div5}>
          <FeedbackContainer
            appointmentId={appointmentId}
            defaultTitle={defaultFeedbackTitle}
            appointmentStatus={status}
            therapistName={therapistInfo.therapistName}
          />
        </div>
      </div>
    </div>
  );
};

export default AppointmentDetail;
