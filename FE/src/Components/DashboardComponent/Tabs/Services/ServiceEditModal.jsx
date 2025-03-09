import React, { useState, useEffect, useCallback } from "react";
import axios from "axios";
import { PUT_SERVICE_API, GET_ALL_SERVICE_TYPES_API } from "../../../../config/apiConfig";
import styles from "./ServiceEditModal.module.css";

const ServiceEditModal = ({ service, onClose, refresh }) => {
  const [formState, setFormState] = useState({
    serviceId: service.serviceId,
    serviceName: service.serviceName,
    description: service.description,
    price: service.price,
    imageUrl: service.imageUrl,
    serviceTypeId: service.serviceTypeId,
  });
  const [serviceTypes, setServiceTypes] = useState([]);
  const [imageFile, setImageFile] = useState(null);

  useEffect(() => {
    const fetchServiceTypes = async () => {
      try {
        const response = await axios.get(GET_ALL_SERVICE_TYPES_API);
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

  const handleFileChange = (e) => {
    setImageFile(e.target.files[0]);
  };

  const handleSubmit = useCallback(
    async (e) => {
      e.preventDefault();
      const formData = new FormData();
      formData.append("serviceName", formState.serviceName);
      formData.append("description", formState.description);
      formData.append("price", parseFloat(formState.price));
      formData.append("serviceTypeId", formState.serviceTypeId);
      if (imageFile) {
        formData.append("image", imageFile);
      }

      try {
        await axios.put(`${PUT_SERVICE_API}/${service.serviceId}`, formData);
        refresh();
        onClose();
      } catch (error) {
        console.error("Error updating service:", error);
      }
    },
    [formState, service.serviceId, refresh, onClose, imageFile]
  );

  const handleDelete = useCallback(async () => {
    try {
      await axios.delete(`${PUT_SERVICE_API}/${service.serviceId}`);
      refresh();
      onClose();
    } catch (error) {
      console.error("Error deleting service:", error);
    }
  }, [service.serviceId, refresh, onClose]);

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent}>
        <h2>Edit Service</h2>
        <div className={styles.contentWrapper}>
          <div className={styles.formSection}>
            <form onSubmit={handleSubmit}>
              {[{ label: "Service Name", name: "serviceName", type: "text" },
                { label: "Description", name: "description", type: "textarea" },
                { label: "Price", name: "price", type: "number" },
                { label: "Image URL", name: "imageUrl", type: "text" }].map(({ label, name, type }) => (
                <label key={name}>
                  {label}:
                  {type === "textarea" ? (
                    <textarea name={name} value={formState[name]} onChange={handleChange} required />
                  ) : (
                    <input name={name} type={type} value={formState[name]} onChange={handleChange} required />
                  )}
                </label>
              ))}
              <label>
                Service Type:
                <select name="serviceTypeId" value={formState.serviceTypeId} onChange={handleChange} required>
                  <option value="">Select a type</option>
                  {serviceTypes.map(({ serviceTypeId, serviceTypeName }) => (
                    <option key={serviceTypeId} value={serviceTypeId}>{serviceTypeName}</option>
                  ))}
                </select>
              </label>
              <label>
                Image:
                <input type="file" name="image" onChange={handleFileChange} />
              </label>
              <div className={styles.buttonContainer}>
                <button className={styles.submitButton} type="submit">Update</button>
                <button className={styles.cancelButton} type="button" onClick={onClose}>Cancel</button>
                <button className={styles.deleteButton} type="button" onClick={handleDelete}>Delete</button>
              </div>
            </form>
          </div>
          <div className={styles.imageSection}>
            <img src={service.imageUrl} alt="Service" className={styles.serviceImage} />
            <p><strong>Service ID:</strong> {service.serviceId}</p>
            <p><strong>Created By:</strong> {service.createdBy || "N/A"}</p>
            <p><strong>Updated By:</strong> {service.updatedBy || "N/A"}</p>
            <p><strong>Created Time:</strong> {service.createdTime || "N/A"}</p>
            <p><strong>Updated Time:</strong> {service.updatedTime || "N/A"}</p>
            <p><strong>Status:</strong> {service.status || "N/A"}</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ServiceEditModal;
