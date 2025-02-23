import React, { useState, useEffect } from "react";
import api from "../../../../config/axios";
import styles from "./ServiceEditModal.module.css";

const ServiceEditModal = ({ service, onClose }) => {
  const [serviceName, setServiceName] = useState(service.serviceName);
  const [description, setDescription] = useState(service.description);
  const [price, setPrice] = useState(service.price);
  const [imageUrl, setImageUrl] = useState(service.imageUrl);
  const [serviceTypeName, setServiceTypeName] = useState("");
  const [serviceTypes, setServiceTypes] = useState([]);
  const [serviceTypeId, setServiceTypeId] = useState(service.serviceTypeId);

  useEffect(() => {
    const fetchServiceTypes = async () => {
      try {
        const response = await api.get("ServiceType/all");
        setServiceTypes(response.data.result);
        const selectedType = response.data.result.find(
          (type) => type.serviceTypeId === service.serviceTypeId
        );
        setServiceTypeName(selectedType.serviceTypeName);
      } catch (error) {
        console.error("Error fetching service types:", error);
      }
    };

    fetchServiceTypes();
  }, [service.serviceTypeId]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const updatedService = {
      serviceId: service.serviceId,
      serviceName,
      description,
      price: parseFloat(price),
      imageUrl,
      serviceTypeId,
    };

    try {
      await api.put("Services/update", updatedService);
      onClose();
    } catch (error) {
      console.error("Error updating service:", error);
    }
  };

  const handleDelete = async () => {
    try {
      await api.delete(`Services/delete/${service.serviceId}`);
      onClose();
    } catch (error) {
      console.error("Error deleting service:", error);
    }
  };

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent}>
        <h2>Edit Service</h2>
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
