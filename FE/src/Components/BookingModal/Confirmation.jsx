/* eslint-disable react/prop-types */
import styles from "./BookingModal.module.css";
const Confirmation = ({
  selectedService,
  selectedTherapist,
  selectedDate,
  selectedTime,
}) => (
  <div className={styles.summary}>
    <h3>Review your appointment details</h3>
    <div className={styles.confirmationGrid}>
      <div className={styles.leftSection}>
        <p>
          <strong>Service:</strong>{" "}
          {selectedService?.serviceName || "Not selected"}
        </p>
        <p>
          <strong>Duration:</strong>{" "}
          {selectedService?.duration
            ? `${selectedService.duration} minutes`
            : "Unspecified"}
        </p>
        <p>
          <strong>Price:</strong>{" "}
          {selectedService?.price
            ? `${selectedService.price} VND`
            : "Unspecified"}
        </p>
      </div>
      <div className={styles.rightSection}>
        <p>
          <strong>Therapist:</strong>{" "}
          {selectedTherapist?.fullName || "Not selected"}
        </p>
        <p>
          <strong>Date:</strong> {selectedDate || "Not selected"}
        </p>
        <p>
          <strong>Time:</strong> {selectedTime || "Not selected"}
        </p>
      </div>
      <div className={styles.bottomSection}>
        <p>
          <strong>Notice: </strong>
          Please make sure all details are correct before confirming your
          appointment. You will be redirected to the payment page.
        </p>
      </div>
    </div>
  </div>
);

export default Confirmation;
