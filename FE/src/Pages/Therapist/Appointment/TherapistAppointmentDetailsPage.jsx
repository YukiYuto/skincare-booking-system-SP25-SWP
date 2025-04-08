/* eslint-disable no-unused-vars */
import React, { useState } from "react";
import styles from "./TherapistAppointmentDetailsPage.module.css";
import SideBarTherapist from "../../../Components/SideBar/SideBarTherapist/SideBarTherapist";
import TherapistAppointmentDetails from "../../../Components/TherapistDashboard/TherapistAppointmentDetails";

const TherapistAppointmentDetailsPage = () => {
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);

  const handleSidebarToggle = (isOpen) => {
    setIsSidebarOpen(isOpen);
  };

  return (
    <div className={styles.container}>
      <div className={styles.sidebarFixed}>
        <SideBarTherapist onToggle={handleSidebarToggle} />
      </div>
      <div
        className={`${styles.content} ${
          isSidebarOpen ? styles.contentOpen : styles.contentClosed
        }`}
      >
        <TherapistAppointmentDetails />
      </div>
    </div>
  );
};

export default TherapistAppointmentDetailsPage;
