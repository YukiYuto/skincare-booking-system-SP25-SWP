import React, { useState } from "react";
import axios from "axios";
import { useSelector } from "react-redux";
import { POST_THERAPIST_API } from "../../../../config/apiConfig";
import styles from "./TherapistAddModal.module.css";

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
  const { user } = useSelector((state) => state.auth);

  // Handle input change
  const handleChange = (e) =>
    setFormState((prev) => ({ ...prev, [e.target.name]: e.target.value }));

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.post(POST_THERAPIST_API, formState, {
        headers: { Authorization: `Bearer ${user.accessToken}` },
      });
      onClose();
      refresh();
    } catch (err) {
      console.error(
        "Error adding therapist:",
        err.response?.data || err.message
      );
    }
  };

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent}>
        <h2>Add New Therapist</h2>
        <form onSubmit={handleSubmit} className={styles.formContainer}>
          {/* Form Fields */}
          {[
            "email",
            "password",
            "confirmPassword",
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
          <label>
            Gender:
            <div className={styles.radioGroup}>
              <label>
                <input
                  type="radio"
                  name="gender"
                  value="Female"
                  checked={formState.gender === "Female"}
                  onChange={handleChange}
                />
                Female
              </label>
              <label>
                <input
                  type="radio"
                  name="gender"
                  value="Male"
                  checked={formState.gender === "Male"}
                  onChange={handleChange}
                />
                Male
              </label>
            </div>
          </label>

          {/* Submit & Cancel */}
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
