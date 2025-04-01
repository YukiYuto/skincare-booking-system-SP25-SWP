import { useState } from "react";
import styles from "./DashboardTherapist.module.css";
import SideBarTherapist from "../../../Components/SideBar/SideBarTherapist/SideBarTherapist";
import TherapistDashboard from "../../../Components/TherapistDashboard/TherapistDashboard";

const DashboardTherapist = () => {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);

    const handleSidebarToggle = (isOpen) => {
        setIsSidebarOpen(isOpen);
    };

    return (
        <div className={styles.container}>
            <div className={styles.sidebarFixed}>
                <SideBarTherapist onToggle={handleSidebarToggle} />
            </div>
            <div className={`${styles.content} ${isSidebarOpen ? styles.contentOpen : styles.contentClosed}`}>
                <TherapistDashboard />
            </div>
        </div>
    );
};

export default DashboardTherapist;
