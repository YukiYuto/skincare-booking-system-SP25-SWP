import React, { useState, useEffect, useRef } from "react";
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
    console.log("Fetching service types from API...");
    axios
      .get(GET_ALL_SERVICE_TYPES_API, {
        headers: { Authorization: `Bearer ${user.accessToken}` },
      })
      .then((res) => {
        console.log("Fetched service types:", res.data.result);
        setServiceTypes(res.data.result);
      })
      .catch((err) =>
        console.error(
          "Error fetching service types:",
          err.response?.data || err.message
        )
      );
  }, [user.accessToken]);

  // Handle form input changes
  const handleChange = (e) =>
    setFormState({ ...formState, [e.target.name]: e.target.value });

  // Handle image upload
  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImageFile(file);
      setPreview(URL.createObjectURL(file));
      console.log("Selected image file:", file.name);
    }
  };

  // Handle service type selection
  const handleSelect = (serviceType) => {
    console.log("Selected service type:", serviceType);
    const updatedSelected = [...selectedServiceTypes, serviceType];
    setSelectedServiceTypes(updatedSelected);
    setServiceTypes(
      serviceTypes.filter(
        (item) => item.serviceTypeId !== serviceType.serviceTypeId
      )
    );
    setShowDropdown(false);

    console.log("Updated selected service types:", updatedSelected);
  };

  // Handle service type deselection
  const handleDeselect = (serviceType) => {
    console.log("Deselected service type:", serviceType);
    const updatedSelected = selectedServiceTypes.filter(
      (item) => item.serviceTypeId !== serviceType.serviceTypeId
    );
    setServiceTypes([...serviceTypes, serviceType]);
    setSelectedServiceTypes(updatedSelected);

    console.log("Updated selected service types:", updatedSelected);
  };

  // Close dropdown when clicking outside
  const handleClickOutside = (event) => {
    if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
      setShowDropdown(false);
    }
  };

  useEffect(() => {
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  // Handle Reset
  const handleReset = () => {
    console.log("Resetting selected service types.");
    setServiceTypes([...serviceTypes, ...selectedServiceTypes]);
    setSelectedServiceTypes([]);
  };

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!imageFile) return alert("Please upload an image.");
    if (selectedServiceTypes.length === 0)
      return alert("Select at least one service type.");

    try {
      console.log("Uploading image...");
      const formData = new FormData();
      formData.append("file", imageFile);
      const imageResponse = await axios.post(POST_FILE_SERVICE_API, formData, {
        headers: { Authorization: `Bearer ${user.accessToken}` },
      });
      console.log("Uploaded image URL:", imageResponse.data.result);

      console.log("Creating new service...");
      // In the handleSubmit function
      const serviceResponse = await axios.post(
        POST_SERVICE_API,
        { ...formState, imageUrl: imageResponse.data.result },
        { headers: { Authorization: `Bearer ${user.accessToken}` } }
      );
      console.log("Created service ID:", serviceResponse.data.result);

      // Extract just the serviceId from the response
      const serviceId = serviceResponse.data.result.serviceId;

      console.log("Associating service with selected service types...");
      await axios.post(
        POST_TYPE_ITEM_API,
        {
          serviceId: serviceId,
          serviceTypeIdList: selectedServiceTypes.map((st) => st.serviceTypeId),
        },
        { headers: { Authorization: `Bearer ${user.accessToken}` } }
      );

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
          {/* Service Name, Description, Price */}
          {["serviceName", "description", "price"].map((name, index) => (
            <label key={index}>
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
                  .filter((st) =>
                    st.serviceTypeName
                      .toLowerCase()
                      .includes(searchTerm.toLowerCase())
                  )
                  .map((st) => (
                    <li
                      key={st.serviceTypeId}
                      className="list-group-item list-group-item-action"
                      onClick={() => handleSelect(st)}
                      style={{ cursor: "pointer" }}
                    >
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
              <button
                type="button"
                className="btn btn-danger btn-sm"
                onClick={handleReset}
              >
                ✖ Reset
              </button>
            )}
          </div>
          <div className={styles.selectedList}>
            {selectedServiceTypes.map((st) => (
              <div key={st.serviceTypeId} className={styles.badge}>
                {st.serviceTypeName}
                <span
                  className={styles.removeIcon}
                  onClick={() => handleDeselect(st)}
                >
                  ✖
                </span>
              </div>
            ))}
          </div>

          {/* Image Upload */}
          <label>Upload Image:</label>
          <input type="file" accept="image/*" onChange={handleImageChange} />
          {preview && (
            <img src={preview} alt="Preview" className={styles.imagePreview} />
          )}

          {/* Submit & Cancel */}
          <div className={styles.buttonGroup}>
            <button type="submit" className={styles.submitButton}>
              Create
            </button>
            <button
              type="button"
              className={styles.cancelButton}
              onClick={onClose}
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ServiceCreateModal;
