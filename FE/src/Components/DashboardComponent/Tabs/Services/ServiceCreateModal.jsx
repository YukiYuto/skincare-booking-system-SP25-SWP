import React, { useState, useEffect, useRef, useCallback } from "react";
import { useSelector } from "react-redux";
import axios from "axios";
import {
  POST_SERVICE_API,
  GET_ALL_SERVICE_TYPES_API,
  POST_FILE_SERVICE_API,
  POST_TYPE_ITEM_API,
} from "../../../../config/apiConfig";
import styles from "./ServiceCreateModal.module.css";

const ServiceCreateModal = ({ onClose, refresh }) => {
  const [formState, setFormState] = useState({
    serviceName: "",
    description: "",
    price: "",
  });
  const [imageFile, setImageFile] = useState(null);
  const [preview, setPreview] = useState(null);
  const [serviceTypes, setServiceTypes] = useState([]);
  const [selectedServiceTypes, setSelectedServiceTypes] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [showDropdown, setShowDropdown] = useState(false);
  const dropdownRef = useRef(null);
  const { user } = useSelector((state) => state.auth);

  // Fetch service types
  useEffect(() => {
    const fetchServiceTypes = async () => {
      try {
        const res = await axios.get(GET_ALL_SERVICE_TYPES_API, {
          headers: { Authorization: `Bearer ${user.accessToken}` },
        });
        setServiceTypes(res.data.result);
      } catch (err) {
        console.error("Error fetching service types:", err.response?.data || err.message);
      }
    };
    fetchServiceTypes();
  }, [user.accessToken]);

  // Handle input change
  const handleChange = (e) => setFormState((prev) => ({ ...prev, [e.target.name]: e.target.value }));

  // Handle image upload
  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImageFile(file);
      setPreview(URL.createObjectURL(file));
    }
  };

  // Handle selecting/deselecting service types
  const updateServiceTypes = useCallback((serviceType, add) => {
    setSelectedServiceTypes((prevSelected) =>
      add ? [...prevSelected, serviceType] : prevSelected.filter((st) => st.serviceTypeId !== serviceType.serviceTypeId)
    );
    setServiceTypes((prevTypes) =>
      add ? prevTypes.filter((st) => st.serviceTypeId !== serviceType.serviceTypeId) : [...prevTypes, serviceType]
    );
  }, []);

  // Close dropdown when clicking outside
  useEffect(() => {
    const handleClickOutside = (e) => {
      if (dropdownRef.current && !dropdownRef.current.contains(e.target)) setShowDropdown(false);
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  // Reset service type selection
  const handleReset = () => {
    setServiceTypes((prev) => [...prev, ...selectedServiceTypes]);
    setSelectedServiceTypes([]);
  };

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!imageFile) return alert("Please upload an image.");
    if (!selectedServiceTypes.length) return alert("Select at least one service type.");

    try {
      // Upload image
      const formData = new FormData();
      formData.append("file", imageFile);
      const { data: imageData } = await axios.post(POST_FILE_SERVICE_API, formData, {
        headers: { Authorization: `Bearer ${user.accessToken}` },
      });

      // Create service
      const { data: serviceData } = await axios.post(
        POST_SERVICE_API,
        { ...formState, imageUrl: imageData.result },
        { headers: { Authorization: `Bearer ${user.accessToken}` } }
      );

      // Associate service with types
      await axios.post(
        POST_TYPE_ITEM_API,
        { serviceId: serviceData.result.serviceId, serviceTypeIdList: selectedServiceTypes.map((st) => st.serviceTypeId) },
        { headers: { Authorization: `Bearer ${user.accessToken}` } }
      );

      onClose();
      refresh();
    } catch (err) {
      console.error("Error creating service:", err.response?.data || err.message);
    }
  };

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent}>
        <h2>Create New Service</h2>
        <form onSubmit={handleSubmit} className={styles.formContainer}>
          {/* Service Name, Description, Price */}
          {["serviceName", "description", "price"].map((name) => (
            <label key={name}>
              {name.charAt(0).toUpperCase() + name.slice(1)}:
              <input
                type={name === "price" ? "number" : "text"}
                name={name}
                value={formState[name]}
                onChange={handleChange}
                required
              />
            </label>
          ))}

          {/* Searchable Service Type Dropdown */}
          <label>Service Types:</label>
          <div ref={dropdownRef} className={styles.dropdownContainer}>
            <input
              type="text"
              placeholder="Search service type..."
              className="form-control"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              onFocus={() => setShowDropdown(true)}
            />
            {showDropdown && (
              <ul className={styles.listGroup}>
                {serviceTypes
                  .filter((st) => st.serviceTypeName.toLowerCase().includes(searchTerm.toLowerCase()))
                  .map((st) => (
                    <li key={st.serviceTypeId} className="list-group-item" onClick={() => updateServiceTypes(st, true)}>
                      {st.serviceTypeName}
                    </li>
                  ))}
              </ul>
            )}
          </div>

          {/* Selected Service Types */}
          <div className={styles.selectedContainer}>
            <h5>Selected Service Types:</h5>
            {selectedServiceTypes.length > 0 && (
              <button type="button" className="btn btn-danger btn-sm" onClick={handleReset}>
                ✖ Reset
              </button>
            )}
          </div>
          <div className={styles.selectedList}>
            {selectedServiceTypes.map((st) => (
              <div key={st.serviceTypeId} className={styles.badge}>
                {st.serviceTypeName}
                <span className={styles.removeIcon} onClick={() => updateServiceTypes(st, false)}>
                  ✖
                </span>
              </div>
            ))}
          </div>

          {/* Image Upload */}
          <label>Upload Image:</label>
          <input type="file" accept="image/*" onChange={handleImageChange} />
          {preview && <img src={preview} alt="Preview" className={styles.imagePreview} />}

          {/* Submit & Cancel */}
          <div className={styles.buttonGroup}>
            <button type="submit" className={styles.submitButton}>
              Create
            </button>
            <button type="button" className={styles.cancelButton} onClick={onClose}>
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ServiceCreateModal;
