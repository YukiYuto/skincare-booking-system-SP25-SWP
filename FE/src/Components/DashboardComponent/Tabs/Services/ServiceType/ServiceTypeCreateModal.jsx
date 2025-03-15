import React, { useState } from "react";
import { apiClient } from "../../../../../config/axios";
import styles from "./ServiceTypeCreateModal.module.css";
import { PUT_SERVICE_TYPE_API } from "../../../../../config/apiConfig";

const ServiceTypeCreateModal = ({ onClose }) => {
  const [formState, setFormState] = useState({
    serviceTypeName: "",
    description: "",
  });
  const [error, setError] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleChange = (e) => {
    setFormState({ ...formState, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    setError("");
    
    try {
      // Use the imported API endpoint constant
      await apiClient.post(PUT_SERVICE_TYPE_API, formState);
      onClose();
    } catch (error) {
      console.error("Error creating service type:", error);
      setError(
        error.response?.status === 404
          ? "API endpoint not found. Please check your server configuration."
          : `Error: ${error.response?.data?.message || error.message}`
      );
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent}>
        <h2>New Service Type</h2>
        {error && <div className={styles.errorMessage}>{error}</div>}
        <form onSubmit={handleSubmit}>
          <label>
            Service Type Name:
            <input
              name="serviceTypeName"
              type="text"
              value={formState.serviceTypeName}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Description:
            <textarea
              name="description"
              value={formState.description}
              onChange={handleChange}
              required
            />
          </label>
          <div className={styles.buttonContainer}>
            <button 
              className={styles.submitButton} 
              type="submit"
              disabled={isSubmitting}
            >
              {isSubmitting ? "Creating..." : "Create"}
            </button>
            <button
              className={styles.btn}
              type="button"
              onClick={onClose}
              disabled={isSubmitting}
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ServiceTypeCreateModal;