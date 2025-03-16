import { useState } from "react";
import styles from "./TherapistSchedule.module.css";
import SideBarTherapist from "../../../Components/SideBar/SideBarTherapist/SideBarTherapist";
import ScheduleTherapistRegister from "../../../Components/ScheduleTherapistRegister/ScheduleTherapistRegister";

const TherapistSchedule = () => {
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
                <ScheduleTherapistRegister />
            </div>
        </div>
    );
};

export default TherapistSchedule;
