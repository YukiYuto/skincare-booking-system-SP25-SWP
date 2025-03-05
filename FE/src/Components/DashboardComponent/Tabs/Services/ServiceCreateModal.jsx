import React, { useState, useEffect } from "react";
import { useDispatch } from "react-redux";
import { createService } from "../../../../redux/Services/ServiceThunk";
import api from "../../../../config/axios";
import styles from "./ServiceCreateModal.module.css";

const ServiceCreateModal = ({ onClose, refresh }) => {
  const dispatch = useDispatch();
  const [formState, setFormState] = useState({
    serviceName: "",
    description: "",
    price: "",
    serviceTypeId: "",
    imageFile: null, // Store image file
  });
  const [serviceTypes, setServiceTypes] = useState([]);
  const [imageFile, setImageFile] = useState(null);
  const [preview, setPreview] = useState(null);

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

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImageFile(file);
      setPreview(URL.createObjectURL(file)); // Show preview
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!imageFile) {
      alert("Please upload an image before submitting.");
      return;
    }

    try {
      const formData = new FormData();
      formData.append("serviceName", formState.serviceName);
      formData.append("description", formState.description);
      formData.append("price", parseFloat(formState.price));
      formData.append("serviceTypeId", formState.serviceTypeId);
      formData.append("image", imageFile); // Append file

      await dispatch(createService(formData)); // Use Redux action

      onClose();
      refresh(); // Refresh the parent component
    } catch (error) {
      console.error("Error creating service:", error);
    }
  };

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent}>
        <h2>Create New Service</h2>
        <form onSubmit={handleSubmit} className={styles.formContainer}>
          {/* Left column for input fields */}
          <div className={styles.formLeft}>
            {[
              { label: "Service Name", name: "serviceName", type: "text" },
              { label: "Description", name: "description", type: "textarea" },
              { label: "Price", name: "price", type: "number" },
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
          </div>

          {/* Right column for image upload */}
          <div className={styles.formRight}>
            <div className={styles.fileInputWrapper}>
              <label className={styles.fileInputLabel}>
                Upload Image:
                <input
                  type="file"
                  accept="image/*"
                  onChange={handleImageChange}
                />
              </label>
            </div>
            {preview && (
              <img
                src={preview}
                alt="Preview"
                className={styles.imagePreview}
              />
            )}
          </div>

          {/* Buttons side by side */}
          <div className={styles.buttonGroup}>
            <button className={styles.submitButton} type="submit">
              Create
            </button>
            <button className={styles.btn} type="button" onClick={onClose}>
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ServiceCreateModal;
