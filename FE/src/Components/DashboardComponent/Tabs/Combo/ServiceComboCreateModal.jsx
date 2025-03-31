import React, { useState, useEffect, useRef, useCallback } from "react";
import { useSelector } from "react-redux";
import axios from "axios";
import {
  POST_SERVICE_COMBO_API,
  POST_FILE_SERVICE_COMBO_API,
  POST_COMBO_ITEM_API,
  GET_ALL_SERVICES_API,
} from "../../../../config/apiConfig";
import styles from "./ServiceComboCreateModal.module.css";

const ServiceComboCreateModal = ({ onClose, refresh }) => {
  const { accessToken } = useSelector((state) => state.auth.user);
  const [formState, setFormState] = useState({
    comboName: "",
    description: "",
    numberOfService: 0,
  });
  const [imageFile, setImageFile] = useState(null);
  const [preview, setPreview] = useState(null);
  const [services, setServices] = useState([]);
  const [selectedServices, setSelectedServices] = useState([]);
  const [searchQuery, setSearchQuery] = useState("");
  const [showDropdown, setShowDropdown] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  
  const dropdownRef = useRef(null);
  const modalRef = useRef(null);

  const fetchServices = useCallback(async () => {
    setLoading(true);
    try {
      const params = new URLSearchParams();
      if (searchQuery) {
        params.append("filterOn", "serviceName");
        params.append("filterQuery", searchQuery);
      }

      const response = await axios.get(`${GET_ALL_SERVICES_API}?${params}`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });

      const selectedIds = selectedServices.map(s => s.serviceId);
      const availableServices = Array.isArray(response.data.result.services)
        ? response.data.result.services.filter(s => !selectedIds.includes(s.serviceId))
        : [];

      setServices(availableServices);
    } catch (err) {
      console.error("Error fetching services:", err.response?.data || err.message);
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }, [searchQuery, accessToken, selectedServices]);

  useEffect(() => {
    fetchServices();
  }, [fetchServices]);

  useEffect(() => {
    setFormState(prev => ({ ...prev, numberOfService: selectedServices.length }));
  }, [selectedServices]);

  useEffect(() => {
    const handleClickOutside = (e) => {
      if (dropdownRef.current && !dropdownRef.current.contains(e.target)) {
        setShowDropdown(false);
      }
    };
    
    const handleModalClick = (e) => {
      if (modalRef.current && modalRef.current.contains(e.target)) {
        e.stopPropagation();
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    document.addEventListener("mousedown", handleModalClick);
    
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
      document.removeEventListener("mousedown", handleModalClick);
    };
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormState(prev => ({ ...prev, [name]: value }));
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImageFile(file);
      setPreview(URL.createObjectURL(file));
    }
  };

  const updateSelectedServices = useCallback((service, add) => {
    if (add) {
      setSelectedServices(prev => [...prev, { ...service, priority: prev.length }]);
      setServices(prev => prev.filter(s => s.serviceId !== service.serviceId));
      setSearchQuery("");
      setShowDropdown(false);
    } else {
      setSelectedServices(prev => prev.filter(s => s.serviceId !== service.serviceId));
      fetchServices();
    }
  }, [fetchServices]);

  const updatePriority = (serviceId, newPriority) => {
    setSelectedServices(prev => {
      const validPriority = Math.max(0, Math.min(prev.length - 1, newPriority));
      const serviceToUpdate = prev.find(s => s.serviceId === serviceId);
      const withoutService = prev.filter(s => s.serviceId !== serviceId);

      const reordered = [
        ...withoutService.slice(0, validPriority),
        { ...serviceToUpdate, priority: validPriority },
        ...withoutService.slice(validPriority),
      ];

      return reordered.map((s, idx) => ({ ...s, priority: idx }));
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!imageFile) return alert("Please upload an image.");
    if (!selectedServices.length) return alert("Select at least one service.");

    try {
      const formData = new FormData();
      formData.append("file", imageFile);
      const { data: imageData } = await axios.post(
        POST_FILE_SERVICE_COMBO_API,
        formData,
        { headers: { Authorization: `Bearer ${accessToken}` } }
      );

      const { data: comboData } = await axios.post(
        POST_SERVICE_COMBO_API,
        {
          comboName: formState.comboName,
          description: formState.description,
          price: 0,
          numberOfService: selectedServices.length,
          imageUrl: imageData.result,
        },
        { headers: { Authorization: `Bearer ${accessToken}` } }
      );

      await axios.post(
        POST_COMBO_ITEM_API,
        {
          serviceComboId: comboData.result.serviceComboId,
          servicePriorityDtos: selectedServices.map(service => ({
            serviceId: service.serviceId,
            priority: service.priority,
          })),
        },
        { headers: { Authorization: `Bearer ${accessToken}` } }
      );

      onClose();
      refresh();
    } catch (err) {
      console.error("Error creating service combo:", err.response?.data || err.message);
      setError(err.message);
    }
  };

  return (
    <div className={styles.modal} onClick={onClose}>
      <div className={styles.modalContent} ref={modalRef} onClick={e => e.stopPropagation()}>
        <h2>Create New Service Combo</h2>
        <form onSubmit={handleSubmit} className={styles.formContainer}>
          <div className={styles.formGroup}>
            <label>
              Combo Name:
              <input
                type="text"
                name="comboName"
                value={formState.comboName}
                onChange={handleChange}
                required
              />
            </label>
            <div>
              <p>Number of Services: {selectedServices.length}</p>
            </div>
          </div>

          <label>
            Description:
            <input
              type="text"
              name="description"
              value={formState.description}
              onChange={handleChange}
              required
            />
          </label>

          <label>Services:</label>
          <div className={styles.dropdownWrapper}>
            <div ref={dropdownRef} className={styles.dropdownContainer}>
              <form onSubmit={e => { e.preventDefault(); fetchServices(); }} className={styles.searchForm}>
                <input
                  type="text"
                  placeholder="Search services..."
                  value={searchQuery}
                  onChange={e => setSearchQuery(e.target.value)}
                  onFocus={() => setShowDropdown(true)}
                  className={styles.searchInput}
                />
              </form>

              {showDropdown && (
                <div className={styles.dropdownResults}>
                  {loading ? (
                    <div className={styles.loadingIndicator}>Loading...</div>
                  ) : error ? (
                    <div className={styles.errorMessage}>Error: {error}</div>
                  ) : services.length === 0 ? (
                    <div className={styles.noResults}>No services found</div>
                  ) : (
                    <ul className={styles.listGroup}>
                      {services.map(service => (
                        <li
                          key={service.serviceId}
                          className={styles.listItem}
                          onClick={() => updateSelectedServices(service, true)}
                        >
                          {service.serviceName} - {service.price}₫
                        </li>
                      ))}
                    </ul>
                  )}
                </div>
              )}
            </div>
            <div className={styles.selectedContainer}>
              {selectedServices.length > 0 && (
                <button
                  type="button"
                  className={styles.resetButton}
                  onClick={() => {
                    setSelectedServices([]);
                    setSearchQuery("");
                    fetchServices();
                  }}
                >
                  <p>Reset✖</p>
                </button>
              )}
            </div>
          </div>

          <div className={styles.selectedList}>
            {selectedServices
              .sort((a, b) => a.priority - b.priority)
              .map(service => (
                <div key={service.serviceId} className={styles.badge}>
                  <span className={styles.priorityControl}>
                    <button
                      type="button"
                      className={styles.priorityButton}
                      onClick={() => updatePriority(service.serviceId, service.priority - 1)}
                      disabled={service.priority === 0}
                    >
                      ↑
                    </button>
                    <span className={styles.priorityValue}>{service.priority + 1}</span>
                    <button
                      type="button"
                      className={styles.priorityButton}
                      onClick={() => updatePriority(service.serviceId, service.priority + 1)}
                      disabled={service.priority === selectedServices.length - 1}
                    >
                      ↓
                    </button>
                  </span>
                  {service.serviceName} - {service.price}₫
                  <span
                    className={styles.removeIcon}
                    onClick={() => updateSelectedServices(service, false)}
                  >
                    ✖
                  </span>
                </div>
              ))}
          </div>

          <label>Upload Image:</label>
          <input type="file" accept="image/*" onChange={handleImageChange} />
          {preview && (
            <img src={preview} alt="Preview" className={styles.imagePreview} />
          )}

          <div className={styles.buttonGroup}>
            <button type="submit" className={styles.submitButton}>Create</button>
            <button type="button" className={styles.cancelButton} onClick={onClose}>Cancel</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ServiceComboCreateModal;