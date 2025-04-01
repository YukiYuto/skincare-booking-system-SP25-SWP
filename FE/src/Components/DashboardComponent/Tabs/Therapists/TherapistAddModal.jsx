import React, { useState, useEffect, useRef, useCallback } from "react";
import axios from "axios";
import { useSelector } from "react-redux";
import {
  POST_THERAPIST_API,
  POST_THERAPIST_SERVICE_TYPE_API,
  GET_ALL_SERVICE_TYPES_API,
  GET_ALL_THERAPISTS_API,
} from "../../../../config/apiConfig";
import styles from "./TherapistAddModal.module.css";
import RPG from "../../RandomPasswordGenerator/RPG";

const TherapistAddModal = ({ onClose, refresh }) => {
  const [formState, setFormState] = useState({
    email: "",
    password: "",
    confirmPassword: "",
    phoneNumber: "",
    fullName: "",
    address: "",
    age: "",
    gender: "Male",
    experience: "",
  });
  const [serviceTypes, setServiceTypes] = useState([]);
  const [selectedServiceTypes, setSelectedServiceTypes] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [showDropdown, setShowDropdown] = useState(false);
  const dropdownRef = useRef(null);
  const { user } = useSelector((state) => state.auth);

  useEffect(() => {
    const fetchServiceTypes = async () => {
      try {
        const res = await axios.get(GET_ALL_SERVICE_TYPES_API, {
          headers: { Authorization: `Bearer ${user.accessToken}` },
        });
        setServiceTypes(res.data.result);
      } catch (err) {
        console.error(
          "Error fetching service types:",
          err.response?.data || err.message
        );
      }
    };
    fetchServiceTypes();
  }, [user.accessToken]);

  const handleChange = (e) =>
    setFormState((prev) => ({ ...prev, [e.target.name]: e.target.value }));

  const handlePasswordGenerate = (newPassword) => {
    setFormState((prev) => ({
      ...prev,
      password: newPassword,
      confirmPassword: newPassword,
    }));
  };

  const updateServiceTypes = useCallback((serviceType, add) => {
    setSelectedServiceTypes((prevSelected) =>
      add
        ? [...prevSelected, serviceType]
        : prevSelected.filter(
            (st) => st.serviceTypeId !== serviceType.serviceTypeId
          )
    );
    setServiceTypes((prevTypes) =>
      add
        ? prevTypes.filter(
            (st) => st.serviceTypeId !== serviceType.serviceTypeId
          )
        : [...prevTypes, serviceType]
    );
  }, []);

  useEffect(() => {
    const handleClickOutside = (e) => {
      if (dropdownRef.current && !dropdownRef.current.contains(e.target))
        setShowDropdown(false);
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const handleReset = () => {
    setServiceTypes((prev) => [...prev, ...selectedServiceTypes]);
    setSelectedServiceTypes([]);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!selectedServiceTypes.length)
      return alert("Select at least one service type.");

    try {
      console.log("Selected service types:", selectedServiceTypes);
      
      // Create therapist
      const { data: therapistData } = await axios.post(
        POST_THERAPIST_API,
        formState,
        {
          headers: { Authorization: `Bearer ${user.accessToken}` },
        }
      );
      
      console.log("Created therapist:", therapistData.result);

      // Get the email of the created therapist
      const createdEmail = therapistData.result.email;
      
      // Fetch all therapists to find the ID matching this email
      const { data: allTherapists } = await axios.get(
        GET_ALL_THERAPISTS_API,
        {
          headers: { Authorization: `Bearer ${user.accessToken}` },
        }
      );
      
      console.log("Searching for therapist with email:", createdEmail);
      
      // Find the therapist ID by matching email
      const therapist = allTherapists.result.find(t => t.email === createdEmail);
      
      if (!therapist) {
        console.error("Therapist not found. All therapists:", allTherapists.result);
        throw new Error("Newly created therapist not found in therapist list");
      }
      
      console.log("Found therapist:", therapist);
      
      // Prepare service type payload
      const serviceTypePayload = {
        therapistId: therapist.skinTherapistId,
        serviceTypeIdList: selectedServiceTypes.map((st) => st.serviceTypeId),
      };
      
      console.log("Sending service type payload:", serviceTypePayload);
      
      // Associate therapist with service types using the correct ID
      const serviceTypeResponse = await axios.post(
        POST_THERAPIST_SERVICE_TYPE_API,
        serviceTypePayload,
        { headers: { Authorization: `Bearer ${user.accessToken}` } }
      );
      
      console.log("Service type association response:", serviceTypeResponse.data);

      onClose();
      refresh();
    } catch (err) {
      console.error(
        "Error adding therapist:",
        err.response?.data || err.message
      );
      alert("Error adding therapist: " + (err.response?.data?.message || err.message));
    }
  };

  const handleClickOutside = (e) => {
    if (e.target.className.includes(styles.modal)) {
      onClose();
    }
  };

  return (
    <div className={styles.modal} onClick={handleClickOutside}>
      <div className={styles.modalContent}>
        <h2>Add New Therapist</h2>
        <form onSubmit={handleSubmit} className={styles.formContainer}>
          {[
            "email",
            "phoneNumber",
            "fullName",
            "address",
            "age",
            "experience",
          ].map((name) => (
            <label key={name}>
              {name.charAt(0).toUpperCase() + name.slice(1)}:
              <input
                type={
                  name === "age" || name === "experience" ? "number" : "text"
                }
                name={name}
                value={formState[name]}
                onChange={handleChange}
                required
              />
            </label>
          ))}

          <div className="flexContainer">
            <label>
              Password:
              <RPG onChange={handlePasswordGenerate} />
            </label>
            <div className={styles.radioGroup}>
              <label>
                <input
                  type="radio"
                  name="gender"
                  value="Female"
                  checked={formState.gender === "Female"}
                  onChange={handleChange}
                />
                <p>Female</p>
              </label>
              <label>
                <input
                  type="radio"
                  name="gender"
                  value="Male"
                  checked={formState.gender === "Male"}
                  onChange={handleChange}
                />
                <p>Male</p>
              </label>
            </div>
          </div>

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
                      className="list-group-item"
                      onClick={() => updateServiceTypes(st, true)}
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
                  onClick={() => updateServiceTypes(st, false)}
                >
                  ✖
                </span>
              </div>
            ))}
          </div>

          <div className={styles.buttonGroup}>
            <button type="submit" className={styles.submitButton}>
              Add
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

export default TherapistAddModal;