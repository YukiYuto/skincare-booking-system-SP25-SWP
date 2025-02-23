import React, { useState, useEffect } from "react";
import api from "../../../../config/axios";
import styles from "./ServiceCreateModal.module.css";

const ServiceCreateModal = ({ onClose }) => {
  const [serviceName, setServiceName] = useState("");
  const [description, setDescription] = useState("");
  const [price, setPrice] = useState("");
  const [imageUrl, setImageUrl] = useState("");
  const [serviceTypeName, setServiceTypeName] = useState("");
  const [serviceTypes, setServiceTypes] = useState([]);
  const [serviceTypeId, setServiceTypeId] = useState("");

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

  const handleSubmit = async (e) => {
    e.preventDefault();
    const newService = {
      serviceName,
      description,
      price: parseFloat(price),
      imageUrl,
      serviceTypeId,
      createdBy: "admin", // Replace with actual user
      createdTime: new Date().toISOString(),
    };

    try {
      await api.post("Services/create", newService);
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
          <label>
            Service Name:
            <input
              type="text"
              value={serviceName}
              onChange={(e) => setServiceName(e.target.value)}
              required
            />
          </label>
          <label>
            Description:
            <textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              required
            />
          </label>
          <label>
            Price:
            <input
              type="number"
              value={price}
              onChange={(e) => setPrice(e.target.value)}
              required
            />
          </label>
          <label>
            Image URL:
            <input
              type="text"
              value={imageUrl}
              onChange={(e) => setImageUrl(e.target.value)}
            />
          </label>
          <label>
            Service Type:
            <select
              value={serviceTypeId}
              onChange={(e) => {
                const selectedType = serviceTypes.find(
                  (type) => type.serviceTypeId === e.target.value
                );
                setServiceTypeId(selectedType.serviceTypeId);
                setServiceTypeName(selectedType.serviceTypeName);
              }}
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
