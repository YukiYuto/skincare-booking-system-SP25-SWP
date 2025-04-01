import React, { useState, useEffect } from "react";
import ReactDOM from "react-dom";
import {
  POST_BOOKING_API,
  GET_CUSTOMER_USER_API,
} from "../../config/apiConfig";
import { apiCall } from "../../utils/apiUtils";
import styles from "./BookingBundleModal.module.css";
import { toast } from "react-toastify"; // Thêm toast để hiển thị thông báo
import { createPaymentLink } from "../../services/paymentService"; // Import service thanh toán
import {
  PAYMENT_CANCEL_URL,
  PAYMENT_CONFIRMATION_URL,
} from "../../config/clientUrlConfig"; // Import URL cấu hình

const BookingBundleModal = ({
  serviceComboId,
  comboPrice,
  services,
  onClose,
}) => {
  const [createdBy, setCreatedBy] = useState("");
  const [customerId, setCustomerId] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

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
        totalPrice: comboPrice,
        createdDate: new Date().toISOString(),
        createdBy,
      },
      orderDetails: [
        {
          serviceComboId: serviceComboId,
          price: comboPrice,
          description: "string",
        },
      ],
    };

    try {
      setIsLoading(true);
      const response = await apiCall("POST", POST_BOOKING_API, payload);
      console.log("Booking successful:", response);
      
      if (response.isSuccess) {
        const orderNumber = response.result.orderNumber;
        localStorage.setItem("orderNumber", orderNumber);
        
        // Tạo liên kết thanh toán
        const paymentResponse = await createPaymentLink(
          orderNumber,
          PAYMENT_CANCEL_URL,
          PAYMENT_CONFIRMATION_URL
        );
        
        if (paymentResponse.isSuccess && paymentResponse.result.result.checkoutUrl) {
          const checkoutUrl = paymentResponse.result.result.checkoutUrl;
          
          toast.success("Booking successful! Redirecting to payment page...");
          
          // Đóng modal và chuyển hướng đến trang thanh toán
          setTimeout(() => {
            onClose();
            window.open(checkoutUrl, "_blank");
          }, 1000);
        } else {
          toast.error("Error occurred while initiating payment. Please try again.");
        }
      } else {
        throw new Error(response.message || "Booking failed");
      }
    } catch (error) {
      console.error("Error booking bundle:", error);
      toast.error(`Failed to book services: ${error.message || "Please try again."}`);
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
              <strong>Customer Id: </strong>
              {customerId}
            </p>
            <p>
              <strong>Total Price:</strong> {comboPrice.toLocaleString()} VND
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
              <p>
                <strong>Services:</strong>
              </p>
              <ul>
                {services.map((service, index) => (
                  <li key={index}>
                    {service.serviceName || `Service ${index + 1}`} <br />
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
          <button onClick={onClose} disabled={isLoading}>
            Cancel
          </button>
        </div>
      </div>
    </div>,
    document.body
  );
};

export default BookingBundleModal;