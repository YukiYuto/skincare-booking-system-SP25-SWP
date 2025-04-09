import { useEffect, useState } from "react";
import styles from "./TherapistCard.module.css";
import { useNavigate } from "react-router-dom";
import { GET_THERAPIST_BY_ID_API } from "../../config/apiConfig";
import TherapistDetailModal from "../TherapistDetail/TherapistDetailModal";
import axios from "axios";

const TherapistCard = ({ skinTherapistId }) => {
  const [therapist, setTherapist] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchTherapist = async () => {
      try {
        const res = await axios.get(
          GET_THERAPIST_BY_ID_API.replace("{therapistId}", skinTherapistId)
        );

        if (res.data?.isSuccess) {
          setTherapist(res.data.result);
        } else {
          console.error("Failed to fetch therapist:", res.data?.message);
        }
      } catch (err) {
        console.error("Error fetching therapist:", err);
      }
    };

    if (skinTherapistId) fetchTherapist();
  }, [skinTherapistId]);

  if (!therapist) return <p>Loading...</p>;

  return (
    <div className={styles.container}>
      <div className={styles.therapistCard} onClick={() => setShowModal(true)}>
        {therapist.imageUrl ? (
          <img
            src={therapist.imageUrl}
            alt={therapist.fullName}
            className={styles.therapistImage}
          />
        ) : (
          <img
            src="https://www.shutterstock.com/image-vector/default-placeholder-doctor-halflength-portrait-600nw-1058724875.jpg"
            alt={therapist.fullName}
            className={styles.therapistImage}
          />
        )}
        <h2 className={styles.therapistName}>
          {therapist.fullName}{" "}
          {therapist.gender === "Male" || therapist.gender === "male"
            ? "M"
            : "F"}
        </h2>

        {/*deo co specialization thi khong hien thi*/}
        <p className={styles.therapistSpecialization}>
          <strong>Specialization:</strong> {therapist.specialization}
        </p>

        <p className={styles.therapistDescription}>
          Experience: {therapist.experience} years
        </p>
      </div>
      {showModal && (
        <TherapistDetailModal
          therapistId={skinTherapistId}
          onClose={() => setShowModal(false)}
        />
      )}
    </div>
  );
};

export default TherapistCard;
