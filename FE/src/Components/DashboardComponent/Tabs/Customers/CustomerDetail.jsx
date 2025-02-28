import React, { useEffect, useState, useRef } from "react";
import api from "../../../../config/axios";
import styles from "./CustomerDetail.module.css";

const CustomerDetail = ({ customer, onClose }) => {
  const [customerDetail, setCustomerDetail] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const modalRef = useRef(null);

  useEffect(() => {
    const fetchCustomerDetail = async () => {
      setLoading(true);
      try {
        const response = await api.get(`Customer/${customer.customerId}`);
        setCustomerDetail(response.data.result);
        setError(null);
      } catch (error) {
        console.error("Error fetching customer detail:", error);
        setError("Failed to load customer details. Please try again.");
      } finally {
        setLoading(false);
      }
    };

    fetchCustomerDetail();

    // Add event listener for escape key
    const handleEscKey = (event) => {
      if (event.key === "Escape") {
        onClose();
      }
    };
    document.addEventListener("keydown", handleEscKey);

    // Handle clicks outside the modal
    const handleOutsideClick = (event) => {
      if (modalRef.current && !modalRef.current.contains(event.target)) {
        onClose();
      }
    };
    document.addEventListener("mousedown", handleOutsideClick);

    // Prevent background scrolling when modal is open
    document.body.style.overflow = "hidden";

    // Clean up
    return () => {
      document.removeEventListener("keydown", handleEscKey);
      document.removeEventListener("mousedown", handleOutsideClick);
      document.body.style.overflow = "auto";
    };
  }, [customer.customerId, onClose]);

  return (
    <div className={styles.modalContainer}>
      <div className={styles.modalOverlay}></div>
      <div className={styles.modalWrapper}>
        <div className={styles.modal} ref={modalRef}>
          <div className={styles.modalHeader}>
            <h2>Customer Detail</h2>
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
                <p>Loading customer details...</p>
              </div>
            ) : error ? (
              <div className={styles.errorContainer}>
                <p>{error}</p>
                <button onClick={() => window.location.reload()}>Retry</button>
              </div>
            ) : customerDetail ? (
              <div className={styles.customerInfo}>
                <div className={styles.infoRow}>
                  <span className={styles.infoLabel}>Name:</span>
                  <span className={styles.infoValue}>{customerDetail.fullName}</span>
                </div>
                <div className={styles.infoRow}>
                  <span className={styles.infoLabel}>Email:</span>
                  <span className={styles.infoValue}>{customerDetail.email}</span>
                </div>
                <div className={styles.infoRow}>
                  <span className={styles.infoLabel}>Age:</span>
                  <span className={styles.infoValue}>{customerDetail.age}</span>
                </div>
                <div className={styles.infoRow}>
                  <span className={styles.infoLabel}>Gender:</span>
                  <span className={styles.infoValue}>{customerDetail.gender}</span>
                </div>
                <div className={styles.infoRow}>
                  <span className={styles.infoLabel}>Phone Number:</span>
                  <span className={styles.infoValue}>{customerDetail.phoneNumber}</span>
                </div>
                <div className={styles.infoRow}>
                  <span className={styles.infoLabel}>Address:</span>
                  <span className={styles.infoValue}>{customerDetail.address}</span>
                </div>
              </div>
            ) : (
              <p>No customer data available</p>
            )}
          </div>
          
          <div className={styles.modalFooter}>
            <button 
              className={styles.closeBtn} 
              onClick={onClose}
            >
              Close
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CustomerDetail;