import React, { useEffect, useRef } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchCustomerById } from "../../../../redux/Customer/CustomerThunk";
import styles from "./CustomerDetail.module.css";

const CustomerDetail = ({ customer, onClose }) => {
  const dispatch = useDispatch();
  const { customerDetail, loading, error } = useSelector((state) => state.customer);
  const modalRef = useRef(null);

  useEffect(() => {
    dispatch(fetchCustomerById(customer.customerId));

    const handleEscKey = (event) => {
      if (event.key === "Escape") onClose();
    };
    document.addEventListener("keydown", handleEscKey);

    const handleOutsideClick = (event) => {
      if (modalRef.current && !modalRef.current.contains(event.target)) onClose();
    };
    document.addEventListener("mousedown", handleOutsideClick);

    document.body.style.overflow = "hidden";

    return () => {
      document.removeEventListener("keydown", handleEscKey);
      document.removeEventListener("mousedown", handleOutsideClick);
      document.body.style.overflow = "auto";
    };
  }, [dispatch, customer.customerId, onClose]);

  return (
    <div className={styles.modalContainer}>
      <div className={styles.modalOverlay}></div>
      <div className={styles.modalWrapper}>
        <div className={styles.modal} ref={modalRef}>
          <div className={styles.modalHeader}>
            <h2>Customer Detail</h2>
            <button className={styles.closeButton} onClick={onClose} aria-label="Close modal">
              &times;
            </button>
          </div>
          <div className={styles.modalBody}>
            {loading ? <p>Loading customer details...</p> : error ? <p>{error}</p> : customerDetail ? (
              <div className={styles.customerInfo}>
                {["fullName", "email", "age", "gender", "phoneNumber", "address"].map((key) => (
                  <div key={key} className={styles.infoRow}>
                    <span className={styles.infoLabel}>{key.charAt(0).toUpperCase() + key.slice(1)}:</span>
                    <span className={styles.infoValue}>{customerDetail[key]}</span>
                  </div>
                ))}
              </div>
            ) : <p>No customer data available</p>}
          </div>
          <div className={styles.modalFooter}>
            <button className={styles.closeBtn} onClick={onClose}>Close</button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CustomerDetail;
