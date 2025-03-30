import React, { useState } from "react";
import axios from "axios";
import { useSelector } from "react-redux";
import { POST_THERAPIST_API } from "../../../../config/apiConfig";
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
  const { user } = useSelector((state) => state.auth);

  const handleChange = (e) =>
    setFormState((prev) => ({ ...prev, [e.target.name]: e.target.value }));

  const handlePasswordGenerate = (newPassword) => {
    setFormState((prev) => ({
      ...prev,
      password: newPassword,
      confirmPassword: newPassword,
    }));
  };

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
