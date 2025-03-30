/* eslint-disable no-unused-vars */
import React, { useState } from "react";
import styles from "./AppointmentDetailsPage.module.css";
import SideBarStaff from "../../../Components/SideBar/SideBarStaff/SideBarStaff";
import AppointmentDetails from "../../../Components/StaffDashboard/Appointment/AppointmentDetails";

const AppointmentDetailsPage = () => {
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);

  const handleSidebarToggle = (isOpen) => {
    setIsSidebarOpen(isOpen);
  };

  return (
    <div className={styles.container}>
      <div className={styles.sidebarFixed}>
        <SideBarStaff onToggle={handleSidebarToggle} />
      </div>
      <div
        className={`${styles.content} ${
          isSidebarOpen ? styles.contentOpen : styles.contentClosed
        }`}
      >
        <AppointmentDetails />
      </div>
    </div>
  );
};

export default AppointmentDetailsPage;
