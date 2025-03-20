import React, { useEffect, useState } from "react";
import axios from "axios";
import style from "./Sidebar.module.css";
import { GET_ALL_THERAPISTS_API } from "../../../config/apiConfig";

const Sidebar = ({ onTherapistChange }) => {
  const [therapists, setTherapists] = useState([]);

  useEffect(() => {
    const fetchTherapists = async () => {
      try {
        const response = await axios.get(GET_ALL_THERAPISTS_API);
        if (response.data.isSuccess) {
          setTherapists(response.data.result);
        }
      } catch (error) {
        console.error("Error fetching therapists:", error);
      }
    };

    fetchTherapists();
  }, []);

  const handleTherapistChange = (e) => {
    const therapistId = e.target.value;
    onTherapistChange(therapistId === "none" ? null : therapistId);
  };

  return (
    <div className={style.container}>
      <div className={style.ServiceOption}>
        <label className={style.checkboxLabel}>
          <p>Service</p>
          <input type="checkbox" value="service" />
        </label>
        <label className={style.checkboxLabel}>
          <p>Combo</p>
          <input type="checkbox" value="combo" />
        </label>
      </div>
      <div className={style.TherapistOption}>
        <label>Appointment</label>
        <select className={style.dropdown} onChange={handleTherapistChange}>
          <option value="none">None</option>
          {therapists.map((therapist) => (
            <option key={therapist.skinTherapistId} value={therapist.skinTherapistId}>
              {therapist.fullName}
            </option>
          ))}
        </select>
      </div>
    </div>
  );
};

export default Sidebar;