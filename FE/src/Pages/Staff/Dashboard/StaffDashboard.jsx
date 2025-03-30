/* eslint-disable no-unused-vars */
import React, { useState } from "react";
import styles from "./StaffDashboard.module.css";
import SideBarStaff from "../../../Components/SideBar/SideBarStaff/SideBarStaff";
import AppointmentTable from "../../../Components/StaffDashboard/Appointment/AppointmentTable";

const StaffDashboard = () => {
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
        <AppointmentTable />
      </div>
    </div>
  );
};

export default StaffDashboard;
