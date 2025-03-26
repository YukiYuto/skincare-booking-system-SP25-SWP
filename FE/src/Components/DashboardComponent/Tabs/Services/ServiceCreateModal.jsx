import React, { useState, useEffect } from "react";
import { useSelector } from "react-redux";
import axios from "axios";
import {
  POST_SERVICE_API,
  GET_ALL_SERVICE_TYPES_API,
  POST_FILE_SERVICE_API,
} from "../../../../config/apiConfig";
import styles from "./ServiceCreateModal.module.css";

const ServiceCreateModal = ({ onClose, refresh }) => {
  const [formState, setFormState] = useState({
    serviceName: "",
    description: "",
    price: "",
    serviceTypeId: "",
  });
  const [imageFile, setImageFile] = useState(null);
  const [preview, setPreview] = useState(null);
  const [serviceTypes, setServiceTypes] = useState([]);
  const { user } = useSelector((state) => state.auth);

  useEffect(() => {
    axios
      .get(GET_ALL_SERVICE_TYPES_API, {
        headers: { Authorization: `Bearer ${user.accessToken}` },
      })
      .then((res) => setServiceTypes(res.data.result))
      .catch((err) =>
        console.error(
          "Error fetching service types:",
          err.response?.data || err.message
        )
      );
  }, [user.accessToken]);

  const handleChange = (e) =>
    setFormState({ ...formState, [e.target.name]: e.target.value });
  
  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImageFile(file);
      setPreview(URL.createObjectURL(file));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!imageFile) return alert("Please upload an image before submitting.");

    const formData = new FormData();
    formData.append("file", imageFile);

    try {
      const imageResponse = await axios.post(POST_FILE_SERVICE_API, formData, {
        headers: { Authorization: `Bearer ${user.accessToken}` },
      });

      // Extract the image URL from the 'result' field of the response
      const imageUrl = imageResponse.data.result;
      console.log("Returned imageUrl:", imageUrl); // Debug log for imageUrl

      const serviceData = {
        ...formState,
        imageUrl,
      };

      console.log("Service data to be submitted:", serviceData); // Debug log for service data

      await axios.post(POST_SERVICE_API, serviceData, {
        headers: { Authorization: `Bearer ${user.accessToken}` },
      });

      onClose();
      refresh();
    } catch (err) {
      console.error(
        "Error creating service:",
        err.response?.data || err.message
      );
    }
  };

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent}>
        <h2>Create New Service</h2>
        <form onSubmit={handleSubmit} className={styles.formContainer}>
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
            Upload Image:
            <input type="file" accept="image/*" onChange={handleImageChange} />
          </label>
          
          {preview && (
            <img src={preview} alt="Preview" className={styles.imagePreview} />
          )}
          
          <div className={styles.buttonGroup}>
            <button type="submit" className={styles.submitButton}>
              Create
            </button>
            <button type="button" className={styles.btn} onClick={onClose}>
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ServiceCreateModal;