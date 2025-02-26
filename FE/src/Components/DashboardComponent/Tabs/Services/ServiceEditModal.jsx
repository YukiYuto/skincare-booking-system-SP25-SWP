import React, { useState, useEffect, useCallback } from "react";
import api from "../../../../config/axios";
import styles from "./ServiceEditModal.module.css";

const ServiceEditModal = ({ service, onClose }) => {
  const [formState, setFormState] = useState({
    serviceId: service.serviceId, // Thêm serviceId vào state
    serviceName: service.serviceName,
    description: service.description,
    price: service.price,
    imageUrl: service.imageUrl,
    serviceTypeId: service.serviceTypeId,
  });
  const [serviceTypes, setServiceTypes] = useState([]);

  useEffect(() => {
    const fetchServiceTypes = async () => {
      try {
        const response = await api.get("ServiceType/all");
        setServiceTypes(response.data.result);
      } catch (error) {
        console.error("Error fetching service types:", error);
      }
    };
    fetchServiceTypes();
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormState((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = useCallback(
    async (e) => {
      e.preventDefault();
      try {
        const response = await api.put("Services/update", {
          ...formState,
          price: parseFloat(formState.price),
        });

        if (response.status === 200) {
          console.log("Service updated successfully", response.data);
          onClose();
        }
      } catch (error) {
        if (error.response) {
          console.error(
            "API Error:",
            error.response.status,
            error.response.data
          );
        } else {
          console.error("Error updating service:", error);
        }
      }
    },
    [formState, onClose]
  );

  const handleDelete = useCallback(async () => {
    try {
      const response = await api.delete(`Services/delete/${service.serviceId}`);
      if (response.status === 200) {
        console.log("Service deleted successfully");
        onClose();
      }
    } catch (error) {
      if (error.response) {
        console.error(
          "Delete API Error:",
          error.response.status,
          error.response.data
        );
      } else {
        console.error("Error deleting service:", error);
      }
    }
  }, [service.serviceId, onClose]);

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent}>
        <h2>Edit Service</h2>
        <form onSubmit={handleSubmit}>
          <label>
            Service Name:
            <input
              type="text"
              name="serviceName"
              value={formState.serviceName}
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
          <label>
            Price:
            <input
              type="number"
              name="price"
              value={formState.price}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Image URL:
            <input
              type="text"
              name="imageUrl"
              value={formState.imageUrl}
              onChange={handleChange}
            />
          </label>
          <label>
            Service Type:
            <select
              name="serviceTypeId"
              value={formState.serviceTypeId}
              onChange={handleChange}
              required
            >
              <option value="">Select a type</option>
              {serviceTypes.map((type) => (
                <option key={type.serviceTypeId} value={type.serviceTypeId}>
                  {type.serviceTypeName}
                </option>
              ))}
            </select>
          </label>
          <button type="submit" className={styles.submitButton}>
            Update
          </button>
          <button type="button" onClick={onClose} className={styles.btn}>
            Cancel
          </button>
          <button type="button" onClick={handleDelete} className={styles.btn}>
            Delete
          </button>
        </form>
      </div>
    </div>
  );
};

export default ServiceEditModal;
