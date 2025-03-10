import React, { useState, useEffect, useCallback } from "react";
import axios from "axios";
import {
  PUT_ORDER_API,
  GET_ALL_CUSTOMERS_API,
  DELETE_ORDER_API,
} from "../../../../config/apiConfig";
import styles from "./OrderEditModal.module.css";

const OrderEditModal = ({ order, onClose, refresh }) => {
  const [formState, setFormState] = useState({
    orderId: order.orderId,
    customerId: order.customerId,
    totalPrice: order.totalPrice,
  });
  const [customers, setCustomers] = useState([]);

  useEffect(() => {
    setFormState({
      orderId: order.orderId,
      customerId: order.customerId,
      totalPrice: order.totalPrice,
    });

    const fetchCustomers = async () => {
      try {
        const response = await axios.get(GET_ALL_CUSTOMERS_API);
        setCustomers(response.data.result);
      } catch (error) {
        console.error("Error fetching customers:", error);
      }
    };
    fetchCustomers();
  }, [order]);

  const handleChange = (e) => {
    setFormState((prev) => ({ ...prev, totalPrice: e.target.value }));
  };

  const handleSubmit = useCallback(
    async (e) => {
      e.preventDefault();
      try {
        await axios.put(`${PUT_ORDER_API}/${order.orderId}`, formState);
        refresh();
        onClose();
      } catch (error) {
        console.error("Error updating order:", error);
      }
    },
    [formState, order.orderId, refresh, onClose]
  );

  const handleDelete = useCallback(async () => {
    try {
      await axios.delete(`${DELETE_ORDER_API}/${order.orderId}`);
      refresh();
      onClose();
    } catch (error) {
      console.error("Error deleting order:", error);
    }
  }, [order.orderId, refresh, onClose]);

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent}>
        <h2>Edit Order</h2>
        <div className={styles.contentWrapper}>
          <div className={styles.formSection}>
            <form onSubmit={handleSubmit}>
              <p>
                <strong>Order Number:</strong> {order.orderNumber}
              </p>
              <label>
                Total Price:
                <input
                  name="totalPrice"
                  type="number"
                  value={formState.totalPrice}
                  onChange={handleChange}
                  required
                />
              </label>
              <p>
                <strong>Customer:</strong> {customers.find(c => c.customerId === order.customerId)?.fullName || "N/A"}
              </p>
              <div className={styles.buttonContainer}>
                <button className={styles.submitButton} type="submit">
                  Update
                </button>
                <button className={styles.cancelButton} type="button" onClick={onClose}>
                  Cancel
                </button>
                <button className={styles.deleteButton} type="button" onClick={handleDelete}>
                  Delete
                </button>
              </div>
            </form>
          </div>
          <div className={styles.orderDetailsSection}>
            <p>
              <strong>Order ID:</strong> {order.orderId}
            </p>
            <p>
              <strong>Created Time:</strong> {order.createdTime || "N/A"}
            </p>
            <p>
              <strong>Updated Time:</strong> {order.updatedTime || "N/A"}
            </p>
            <p>
              <strong>Status:</strong> {order.status || "N/A"}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default OrderEditModal;