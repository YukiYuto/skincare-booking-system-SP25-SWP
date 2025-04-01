import React, { useState, useEffect, useCallback, useRef } from "react";
import axios from "axios";
import { useSelector } from "react-redux";
import {
  PUT_SERVICE_API,
  GET_ALL_SERVICE_TYPES_API,
  POST_FILE_SERVICE_API,
  PUT_TYPE_ITEM_API,
} from "../../../../config/apiConfig";
import styles from "./ServiceEditModal.module.css";

const ServiceEditModal = ({ service, onClose, refresh }) => {
  const [formState, setFormState] = useState({ ...service });
  const [allServiceTypes, setAllServiceTypes] = useState([]);
  const [selectedServiceTypes, setSelectedServiceTypes] = useState([]);
  const [imageFile, setImageFile] = useState(null);
  const [preview, setPreview] = useState(service.imageUrl);
  const [searchTerm, setSearchTerm] = useState("");
  const [showDropdown, setShowDropdown] = useState(false);
  const dropdownRef = useRef(null);
  const { user } = useSelector((state) => state.auth);

  useEffect(() => {
    axios
      .get(GET_ALL_SERVICE_TYPES_API, {
        headers: { Authorization: `Bearer ${user.accessToken}` },
      })
      .then((res) => {
        const serviceTypesData = res.data.result;
        setAllServiceTypes(serviceTypesData);

        if (service.serviceTypeIds && service.serviceTypeIds.length > 0) {
          const initialSelected = service.serviceTypeIds
            .map((id) => serviceTypesData.find((st) => st.serviceTypeId === id))
            .filter(Boolean);
          setSelectedServiceTypes(initialSelected);

          setAllServiceTypes(
            serviceTypesData.filter(
              (st) => !service.serviceTypeIds.includes(st.serviceTypeId)
            )
          );
        }
      })
      .catch((err) => console.error("Error fetching service types:", err));
  }, [user.accessToken, service.serviceTypeIds]);

  const handleChange = (e) =>
    setFormState({ ...formState, [e.target.name]: e.target.value });

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImageFile(file);
      setPreview(URL.createObjectURL(file));
    }
  };

  const handleSelect = (serviceType) => {
    setSelectedServiceTypes([...selectedServiceTypes, serviceType]);
    setAllServiceTypes(
      allServiceTypes.filter(
        (st) => st.serviceTypeId !== serviceType.serviceTypeId
      )
    );
    setShowDropdown(false);
  };
  
  const handleDeselect = (serviceType) => {
    setSelectedServiceTypes(
      selectedServiceTypes.filter(
        (st) => st.serviceTypeId !== serviceType.serviceTypeId
      )
    );
    setAllServiceTypes([...allServiceTypes, serviceType]);
  };

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setShowDropdown(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const handleSubmit = useCallback(
    async (e) => {
      e.preventDefault();
      let imageUrl = formState.imageUrl;

      if (imageFile) {
        const formData = new FormData();
        formData.append("file", imageFile);
        try {
          const imageResponse = await axios.post(
            POST_FILE_SERVICE_API,
            formData,
            {
              headers: {
                Authorization: `Bearer ${user.accessToken}`,
                "Content-Type": "multipart/form-data",
              },
            }
          );
          imageUrl =
            imageResponse.data.result ||
            imageResponse.data.imageUrl ||
            imageResponse.data;
        } catch (error) {
          console.error("Error uploading image:", error);
        }
      }

      const serviceTypeIds = selectedServiceTypes.map((st) => st.serviceTypeId);

      await axios.put(
        PUT_SERVICE_API,
        {
          ...formState,
          imageUrl,
          serviceTypeIds,
        },
        { headers: { Authorization: `Bearer ${user.accessToken}` } }
      );

      await axios.put(
        PUT_TYPE_ITEM_API,
        { serviceId: service.serviceId, serviceTypeIdList: serviceTypeIds },
        { headers: { Authorization: `Bearer ${user.accessToken}` } }
      );

      refresh();
      onClose();
    },
    [
      formState,
      imageFile,
      selectedServiceTypes,
      refresh,
      onClose,
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
        <form onSubmit={handleSubmit} className={styles.formContainer}>
          {["serviceName", "description", "price"].map((name) => (
            <label key={name}>
              {name.charAt(0).toUpperCase() + name.slice(1)}:
              {name === "description" ? (
                <textarea
                  name={name}
                  value={formState[name]}
                  onChange={handleChange}
                  required
                />
              ) : (
                <input
                  name={name}
                  type={name === "price" ? "number" : "text"}
                  value={formState[name]}
                  onChange={handleChange}
                  required
                />
              )}
            </label>
          ))}

          <label>Service Types:</label>
          <div ref={dropdownRef} className={styles.dropdownContainer}>
            <input
              type="text"
              placeholder="Search service type..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              onFocus={() => setShowDropdown(true)}
            />
            {showDropdown && (
              <ul className={styles.listGroup}>
                {allServiceTypes
                  .filter((st) =>
                    st.serviceTypeName
                      .toLowerCase()
                      .includes(searchTerm.toLowerCase())
                  )
                  .map((st) => (
                    <li
                      key={st.serviceTypeId}
                      onClick={() => handleSelect(st)}
                      className={styles.listItem}
                    >
                      {st.serviceTypeName}
                    </li>
                  ))}
              </ul>
            )}
          </div>
          <div className={styles.selectedContainer}>
            <h5>Selected Service Types:</h5>
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

          <label>
            Image:
            <input type="file" onChange={handleFileChange} />
          </label>
          {preview && (
            <img src={preview} alt="Preview" className={styles.imagePreview} />
          )}

          <div className={styles.buttonGroup}>
            <button type="submit" className={styles.submitButton}>
              Update
            </button>
            <button
              type="button"
              className={styles.cancelButton}
              onClick={onClose}
            >
              Cancel
            </button>
            <button
              type="button"
              className={styles.deleteButton}
              onClick={handleDelete}
            >
              Delete
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ServiceEditModal;
