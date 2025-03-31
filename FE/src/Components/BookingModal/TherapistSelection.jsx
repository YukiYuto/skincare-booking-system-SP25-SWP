/* eslint-disable react/prop-types */
import styles from "./BookingModal.module.css";

const TherapistSelection = ({
  therapists,
  selectedTherapist,
  setSelectedTherapist,
}) => {
  return (
    <div className={styles.stepContainer}>
      <select
        value={selectedTherapist ? selectedTherapist.skinTherapistId : ""}
        onChange={(e) => {
          const therapist = therapists.find(
            (t) => t.skinTherapistId === e.target.value
          );
          setSelectedTherapist(therapist || null);
        }}
      >
        <option value="">-- Select Therapist --</option>
        {therapists.map((therapist) => (
          <option
            key={therapist.skinTherapistId}
            value={therapist.skinTherapistId}
          >
            {therapist.fullName}
          </option>
        ))}
      </select>
      {selectedTherapist && (
        <div className={styles.detailContainer}>
          <img
            src={selectedTherapist.imageUrl}
            alt="Therapist"
            className={styles.image}
          />
          <div className={styles.info}>
            <h3>{selectedTherapist.fullName}</h3>
            <p>
              <strong>Experience:</strong> {selectedTherapist.experience} years
            </p>
          </div>
        </div>
      )}
    </div>
  );
};

export default TherapistSelection;
