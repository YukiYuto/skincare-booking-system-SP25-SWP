import React, { useEffect, useState } from "react";
import styles from "./CustomerProfileModal.module.css";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { GET_CUSTOMER_BY_ID_API } from "../../config/apiConfig";
import axios from "axios";

const CustomerProfileModal = (customer, onClose) => {
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
      <div className={styles.modalContent}>
        <span className={styles.close}>&times;</span>
        <h2>Customer Profile</h2>
      </div>
    </div>
  );
};

export default CustomerProfileModal;
