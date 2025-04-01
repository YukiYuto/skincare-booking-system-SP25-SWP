import React, { useState, useEffect } from "react";
import ReactDOM from "react-dom";
import { POST_BOOKING_API, GET_CUSTOMER_USER_API } from "../../config/apiConfig";
import { apiCall } from "../../utils/apiUtils";
import styles from "./BookingBundleModal.module.css";

const BookingBundleModal = ({ services, onClose }) => {
  const [totalPrice, setTotalPrice] = useState(0);
  const [createdBy, setCreatedBy] = useState("");
  const [customerId, setCustomerId] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (services && services.length > 0) {
      const sum = services.reduce(
        (total, service) => total + (service.price || 0),
        0
      );
      setTotalPrice(sum);
    }
  }, [services]);

  useEffect(() => {
    const handleEscKey = (e) => {
      if (e.key === "Escape") {
        onClose();
      }
    };

    window.addEventListener("keydown", handleEscKey);
    return () => window.removeEventListener("keydown", handleEscKey);
  }, [onClose]);

  useEffect(() => {
    document.body.style.overflow = "hidden";
    return () => {
      document.body.style.overflow = "auto";
    };
  }, []);

  useEffect(() => {
    const fetchCustomerId = async () => {
      setIsLoading(true);
      try {
        const response = await apiCall("GET", GET_CUSTOMER_USER_API);
        
        if (response.isSuccess) {
          setCustomerId(response.result);
        } else {
          throw new Error("Failed to get valid customer ID");
        }
      } catch (error) {
        console.error("Error fetching customer ID:", error);
        setCustomerId(null);
      } finally {
        setIsLoading(false);
      }
    };

    fetchCustomerId();
  }, []);

  const handleBooking = async () => {
    if (!createdBy.trim()) {
      alert("Please enter your name");
      return;
    }

    if (!customerId) {
      alert("Customer ID not available. Please try again later.");
      return;
    }

    const payload = {
      order: {
        customerId,
        totalPrice,
        createdDate: new Date().toISOString(),
        createdBy,
      },
      orderDetails: services.map((service) => ({
        serviceId: service.serviceId,
        serviceComboId: service.serviceComboId,
        price: service.price || 0,
        description: service.description || "",
      })),
    };

    try {
      setIsLoading(true);
      const response = await apiCall("POST", POST_BOOKING_API, payload);
      console.log("Booking successful:", response);
      onClose();
    } catch (error) {
      console.error("Error booking bundle:", error);
      alert(`Failed to book services: ${error.message || "Please try again."}`);
    } finally {
      setIsLoading(false);
    }
  };

  return ReactDOM.createPortal(
    <div
      className={styles.modal}
      onClick={(e) => {
        if (e.target === e.currentTarget) {
          onClose();
        }
      }}
    >
      <div className={styles.content}>
        <h3 className={styles.modalTitle}>Book Service Bundle</h3>
        <div className={styles.stepContainer}>
          <div className={styles.detailContainer}>
            <p>
              <strong>Customer ID:</strong>{" "}
              {isLoading ? "Loading..." : customerId || "Not available"}
            </p>
            <p>
              <strong>Total Price:</strong> {totalPrice.toLocaleString()} VND
            </p>
            <p>
              <strong>Created By:</strong>
              <input
                type="text"
                value={createdBy}
                onChange={(e) => setCreatedBy(e.target.value)}
                placeholder="Enter your name"
                className={styles.input}
              />
            </p>
          </div>
          {services && services.length > 0 ? (
            <div>
              <p><strong>Services:</strong></p>
              <ul>
                {services.map((service, index) => (
                  <li key={index}>
                    <strong>Name:</strong> {service.serviceName || `Service ${index + 1}`} <br />
                    <strong>Service ID:</strong> {service.serviceId} <br />
                    <strong>Combo ID:</strong> {service.serviceComboId} <br />
                    <strong>Price:</strong> {(service.price || 0).toLocaleString()} VND <br />
                    <strong>Description:</strong> {service.description || "N/A"}
                  </li>
                ))}
              </ul>
            </div>
          ) : (
            <p>No services selected</p>
          )}
        </div>
        <div className={styles.buttons}>
          <button onClick={handleBooking} disabled={isLoading || !customerId}>
            {isLoading ? "Processing..." : "Confirm Booking"}
          </button>
          <button onClick={onClose} disabled={isLoading}>Cancel</button>
        </div>
      </div>
    </div>,
    document.body
  );
};

export default BookingBundleModal;