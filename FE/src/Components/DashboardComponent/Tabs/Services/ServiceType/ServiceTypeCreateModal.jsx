import React, { useState } from "react";
import api from "../../../../../config/axios";
import styles from "./ServiceTypeCreateModal.module.css";

const ServiceTypeCreateModal = ({ onClose }) => {
  const [formState, setFormState] = useState({
    serviceTypeName: "",
    description: "",
  });

  const handleChange = (e) => {
    setFormState({ ...formState, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await api.post("ServiceType/create", {
        ...formState,
      });
      onClose();
    } catch (error) {
      console.error("Error creating service type:", error);
    }
  };

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent}>
        <h2>New Service Type</h2>
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
          <button className={styles.submitButton} type="submit">
            Create
          </button>
          <button className={styles.btn} type="button" onClick={onClose}>
            Cancel
          </button>
        </form>
      </div>
    </div>
  );
};

export default ServiceTypeCreateModal;
