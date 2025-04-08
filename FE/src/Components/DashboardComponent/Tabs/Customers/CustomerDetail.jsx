import React, { useEffect, useRef, useState } from "react";
import { useSelector } from "react-redux";
import axios from "axios";
import {
  GET_CUSTOMER_BY_ID_API,
  GET_ALL_APPOINTMENTS_API,
} from "../../../../config/apiConfig";
import styles from "./CustomerDetail.module.css";

const CustomerDetail = ({ customer, onClose }) => {
  const [customerDetail, setCustomerDetail] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const modalRef = useRef(null);
  const { user } = useSelector((state) => state.auth);

  useEffect(() => {
    const fetchCustomerDetail = async () => {
      try {
        const response = await axios.get(
          GET_CUSTOMER_BY_ID_API.replace("{customerId}", customer.customerId),
          {
            headers: {
              Authorization: `Bearer ${user.accessToken}`,
            },
          }
        );
        setCustomerDetail(response.data.result);
      } catch (err) {
        if (err.response && err.response.status === 404) {
          setError("Customer not found (404)");
        } else {
          setError(err.message);
        }
      } finally {
        setLoading(false);
      }
    };

    fetchCustomerDetail();

    const handleEscKey = (event) => {
      if (event.key === "Escape") onClose();
    };
    document.addEventListener("keydown", handleEscKey);

    const handleOutsideClick = (event) => {
      if (modalRef.current && !modalRef.current.contains(event.target))
        onClose();
    };
    document.addEventListener("mousedown", handleOutsideClick);

    document.body.style.overflow = "hidden";

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
              <p>Loading customer details...</p>
            ) : error ? (
              <p>{error}</p>
            ) : customerDetail ? (
              <div className={styles.customerInfo}>
                {[
                  "fullName",
                  "email",
                  "age",
                  "gender",
                  "phoneNumber",
                  "address",
                ].map((key) => (
                  <div key={key} className={styles.infoRow}>
                    <span className={styles.infoLabel}>
                      {key.charAt(0).toUpperCase() + key.slice(1)}:
                    </span>
                    <span className={styles.infoValue}>
                      {customerDetail[key]}
                    </span>
                  </div>
                ))}
              </div>
            ) : (
              <p>No customer data available</p>
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

export default CustomerDetail;
