import { useEffect, useState } from "react";
import axios from "axios";
import styles from "./TherapistDetailModal.module.css";
import { GET_THERAPIST_BY_ID_API } from "../../config/apiConfig";

const TherapistDetailModal = ({ therapistId, onClose }) => {
  const [therapist, setTherapist] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (therapistId) {
      axios
        .get(GET_THERAPIST_BY_ID_API.replace("{therapistId}", therapistId))
        .then((res) => {
          setTherapist(res.data.result);
          setLoading(false);
        })
        .catch((err) => {
          setError(err.message);
          setLoading(false);
        });
    }
  }, [therapistId]);

  if (!therapistId) return null;

  return (
    <div className={styles.modalOverlay}>
      <div className={styles.modal}>
        <button className={styles.closeBtn} onClick={onClose}>
          ✕
        </button>
        {loading && <div className={styles.loading}>Loading...</div>}
        {error && <div className={styles.error}>Error: {error}</div>}
        {therapist && (
          <>
            <img
              src={
                therapist.imageUrl ||
                "https://www.shutterstock.com/image-vector/default-placeholder-doctor-halflength-portrait-600nw-1058724875.jpg"
              }
              alt={therapist.fullName}
              className={styles.image}
            />
            <h2>{therapist.fullName}</h2>
            <table className={styles.infoTable}>
              <tbody>
                <tr>
                  <td>Age</td>
                  <td>
                    {therapist.age ? (
                      therapist.age
                    ) : (
                      <span>Pretty legal age</span>
                    )}
                  </td>
                </tr>
                <tr>
                  <td>Gender</td>
                  <td>{therapist.gender}</td>
                </tr>
                <tr>
                  <td>Experience</td>
                  <td>{therapist.experience}</td>
                </tr>
                <tr>
                  <td>Phone</td>
                  <td>{therapist.phoneNumber}</td>
                </tr>
                <tr>
                  <td>Email</td>
                  <td>{therapist.email}</td>
                </tr>
              </tbody>
            </table>
          </>
        )}
      </div>
    </div>
  );
};

export default TherapistDetailModal;
