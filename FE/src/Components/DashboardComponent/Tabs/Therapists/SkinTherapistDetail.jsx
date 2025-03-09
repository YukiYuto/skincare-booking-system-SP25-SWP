import React, { useEffect, useState, useRef } from "react";
import { GET_THERAPIST_BY_ID_API } from "../../../../config/apiConfig";
import styles from "./SkinTherapistDetail.module.css";

const SkinTherapistDetail = ({ therapist, onClose }) => {
  const [therapistDetail, setTherapistDetail] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const modalRef = useRef(null);

  useEffect(() => {
    console.log("Full therapist object:", therapist);
  
    if (!therapist?.skinTherapistId) {
      setError("Invalid therapist ID");
      setLoading(false);
      return;
    }
  
    const fetchTherapistDetail = async () => {
      setLoading(true);
      try {
        const response = await fetch(
          GET_THERAPIST_BY_ID_API.replace("{therapistId}", therapist.skinTherapistId)
        );
        const data = await response.json();
  
        if (data.isSuccess) {
          setTherapistDetail(data.result);
          setError(null);
        } else {
          setError("Failed to load therapist details.");
        }
      } catch (error) {
        console.error("Error fetching therapist detail:", error);
        setError("Failed to load therapist details.");
      } finally {
        setLoading(false);
      }
    };
  
    fetchTherapistDetail();

    const handleEscKey = (event) => {
      if (event.key === "Escape") {
        onClose();
      }
    };
    document.addEventListener("keydown", handleEscKey);

    const handleOutsideClick = (event) => {
      if (modalRef.current && !modalRef.current.contains(event.target)) {
        onClose();
      }
    };
    document.addEventListener("mousedown", handleOutsideClick);

    document.body.style.overflow = "hidden";

    return () => {
      document.removeEventListener("keydown", handleEscKey);
      document.removeEventListener("mousedown", handleOutsideClick);
      document.body.style.overflow = "auto";
    };
  }, [therapist, onClose]);

  return (
    <div className={styles.modalContainer}>
      <div className={styles.modalOverlay}></div>
      <div className={styles.modalWrapper}>
        <div className={styles.modal} ref={modalRef}>
          <div className={styles.modalHeader}>
            <h2>Therapist Detail</h2>
            <button
              className={styles.closeButton}
              onClick={onClose}
              aria-label="Close modal"
            >
              &times;
            </button>
          </div>

          <div className={styles.modalBody}>
            {loading ? (
              <div className={styles.loadingContainer}>
                <p>Loading therapist details...</p>
              </div>
            ) : error ? (
              <div className={styles.errorContainer}>
                <p>{error}</p>
                <button onClick={() => window.location.reload()}>Retry</button>
              </div>
            ) : therapistDetail ? (
              <div className={styles.therapistInfo}>
                <div className={styles.infoRow}>
                  <span className={styles.infoLabel}>Name:</span>
                  <span className={styles.infoValue}>
                    {therapistDetail.fullName}
                  </span>
                </div>
                <div className={styles.infoRow}>
                  <span className={styles.infoLabel}>Email:</span>
                  <span className={styles.infoValue}>
                    {therapistDetail.email}
                  </span>
                </div>
                <div className={styles.infoRow}>
                  <span className={styles.infoLabel}>Age:</span>
                  <span className={styles.infoValue}>
                    {therapistDetail.age}
                  </span>
                </div>
                <div className={styles.infoRow}>
                  <span className={styles.infoLabel}>Gender:</span>
                  <span className={styles.infoValue}>
                    {therapistDetail.gender}
                  </span>
                </div>
                <div className={styles.infoRow}>
                  <span className={styles.infoLabel}>Phone:</span>
                  <span className={styles.infoValue}>
                    {therapistDetail.phoneNumber}
                  </span>
                </div>
                <div className={styles.infoRow}>
                  <span className={styles.infoLabel}>Experience:</span>
                  <span className={styles.infoValue}>
                    {therapistDetail.experience} years
                  </span>
                </div>
              </div>
            ) : (
              <p>No therapist data available</p>
            )}
          </div>

          <div className={styles.modalFooter}>
            <button className={styles.closeBtn} onClick={onClose}>
              Close
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default SkinTherapistDetail;
