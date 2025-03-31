import React, { useState } from "react";
import { Outlet } from "react-router-dom";
import Sidebar from "../../Components/DashboardComponent/OuterSection/Sidebar";
import Header from "../../Components/DashboardComponent/OuterSection/Header";
import styles from "./Dashboard.module.css";

const Dashboard = () => {
  const [sidebarExpanded, setSidebarExpanded] = useState(false);
  
  const handleSidebarExpand = (expanded) => {
    setSidebarExpanded(expanded);
  };

  return (
    <div className={styles.dashboardContainer}>
      <Sidebar onExpandChange={handleSidebarExpand} />
      <div className={styles.mainContent}>
        <Header />
        <div className={styles.contentArea}>
          <Outlet />
        </div>
      </div>
    </div>
  );
};

export default Dashboard;