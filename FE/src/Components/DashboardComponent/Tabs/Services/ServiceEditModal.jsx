import React, { useState, useEffect, useCallback } from "react";
import axios from "axios";
import { useSelector } from "react-redux";
import {
  PUT_SERVICE_API,
  GET_ALL_SERVICE_TYPES_API,
  POST_FILE_SERVICE_API,
} from "../../../../config/apiConfig";
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
  const [preview, setPreview] = useState(service.imageUrl);
  const { user } = useSelector((state) => state.auth);

  // For debugging - let's see what the endpoint URL is
  useEffect(() => {
    console.log("File upload endpoint:", POST_FILE_SERVICE_API);
  }, []);

  useEffect(() => {
    const fetchServiceTypes = async () => {
      try {
        const response = await axios.get(GET_ALL_SERVICE_TYPES_API, {
          headers: { Authorization: `Bearer ${user.accessToken}` },
        });
        setServiceTypes(response.data.result);
      } catch (error) {
        console.error("Error fetching service types:", error);
      }
    };
    fetchServiceTypes();
  }, [user.accessToken]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormState((prev) => ({ ...prev, [name]: value }));
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImageFile(file);
      setPreview(URL.createObjectURL(file));
    }
  };

  const handleSubmit = useCallback(
    async (e) => {
      e.preventDefault();
      let imageUrl = formState.imageUrl;

      if (imageFile) {
        const formData = new FormData();
        formData.append("file", imageFile);

        try {
          // Try with explicit URL instead of the imported constant
          const fileUploadUrl = POST_FILE_SERVICE_API;
          console.log("Attempting to upload with URL:", fileUploadUrl);

          const imageResponse = await axios.post(fileUploadUrl, formData, {
            headers: {
              Authorization: `Bearer ${user.accessToken}`,
              "Content-Type": "multipart/form-data",
            },
          });

          console.log("Image upload response:", imageResponse.data);

          // Check different possible response structures
          if (imageResponse.data.result) {
            imageUrl = imageResponse.data.result;
          } else if (imageResponse.data.imageUrl) {
            imageUrl = imageResponse.data.imageUrl;
          } else if (typeof imageResponse.data === "string") {
            imageUrl = imageResponse.data;
          }

          console.log("Using image URL:", imageUrl);
        } catch (error) {
          console.error("Error uploading image:", error);
          // Continue with the existing image URL if upload fails
          console.log("Continuing with existing image URL:", imageUrl);
        }
      }

      const serviceData = {
        ...formState,
        imageUrl,
      };

      console.log("Service data to be submitted:", serviceData);
      try {
        await axios.put(`${PUT_SERVICE_API}`, serviceData, {
          headers: { Authorization: `Bearer ${user.accessToken}` },
        });
        refresh();
        onClose();
      } catch (error) {
        console.error("Error updating service:", error);
      }
    },
    [
      formState,
      service.serviceId,
      refresh,
      onClose,
      imageFile,
      user.accessToken,
    ]
  );

  const handleDelete = useCallback(async () => {
    try {
      await axios.delete(`${PUT_SERVICE_API}/${service.serviceId}`, {
        headers: { Authorization: `Bearer ${user.accessToken}` },
      });
      refresh();
      onClose();
    } catch (error) {
      console.error("Error deleting service:", error);
    }
  }, [service.serviceId, refresh, onClose, user.accessToken]);

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent}>
        <h2>Edit Service</h2>
        <div className={styles.contentWrapper}>
          <div className={styles.formSection}>
            <form onSubmit={handleSubmit}>
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
              <label>
                Image:
                <input type="file" name="image" onChange={handleFileChange} />
              </label>
              {preview && (
                <img
                  src={preview}
                  alt="Preview"
                  className={styles.imagePreview}
                />
              )}
              <div className={styles.buttonContainer}>
                <button className={styles.submitButton} type="submit">
                  Update
                </button>
                <button
                  className={styles.cancelButton}
                  type="button"
                  onClick={onClose}
                >
                  Cancel
                </button>
                <button
                  className={styles.deleteButton}
                  type="button"
                  onClick={handleDelete}
                >
                  Delete
                </button>
              </div>
            </form>
          </div>
          <div className={styles.imageSection}>
            <img
              src={service.imageUrl}
              alt="Service"
              className={styles.serviceImage}
            />
            <p>
              <strong>Service ID:</strong> {service.serviceId}
            </p>
            <p>
              <strong>Created By:</strong> {service.createdBy || "N/A"}
            </p>
            <p>
              <strong>Updated By:</strong> {service.updatedBy || "N/A"}
            </p>
            <p>
              <strong>Created Time:</strong> {service.createdTime || "N/A"}
            </p>
            <p>
              <strong>Updated Time:</strong> {service.updatedTime || "N/A"}
            </p>
            <p>
              <strong>Status:</strong> {service.status || "N/A"}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ServiceEditModal;
