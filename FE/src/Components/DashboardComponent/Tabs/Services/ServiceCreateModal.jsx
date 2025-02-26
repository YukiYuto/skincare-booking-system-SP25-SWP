import React, { useState, useEffect } from "react";
import api from "../../../../config/axios";
import styles from "./ServiceCreateModal.module.css";

const ServiceCreateModal = ({ onClose }) => {
  const [formState, setFormState] = useState({
    serviceName: "",
    description: "",
    price: "",
    imageUrl: "",
    serviceTypeId: "",
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
    setFormState({ ...formState, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await api.post("Services/create", {
        ...formState,
        price: parseFloat(formState.price),
        createdBy: "admin",
        createdTime: new Date().toISOString(),
      });
      onClose();
    } catch (error) {
      console.error("Error creating service:", error);
    }
  };

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent}>
        <h2>Create New Service</h2>
        <form onSubmit={handleSubmit}>
          {[
            { label: "Service Name", name: "serviceName", type: "text" },
            { label: "Description", name: "description", type: "textarea" },
            { label: "Price", name: "price", type: "number" },
            { label: "Image URL", name: "imageUrl", type: "text" },
          ].map(({ label, name, type }) => (
            <label key={name}>
              {label}:
              {type === "textarea" ? (
                <textarea
                  name={name}
                  value={formState[name]}
                  onChange={handleChange}
                  required
                />
              ) : (
                <input
                  name={name}
                  type={type}
                  value={formState[name]}
                  onChange={handleChange}
                  required
                />
              )}
            </label>
          ))}
          <label>
            Service Type:
            <select
              name="serviceTypeId"
              value={formState.serviceTypeId}
              onChange={handleChange}
              required
            >
              <option value="">Select a type</option>
              {serviceTypes.map(({ serviceTypeId, serviceTypeName }) => (
                <option key={serviceTypeId} value={serviceTypeId}>
                  {serviceTypeName}
                </option>
              ))}
            </select>
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

export default ServiceCreateModal;
