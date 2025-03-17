import { useState } from "react";
import styles from "./ScheduleManagement.module.css";
import SideBarStaff from "../../../Components/SideBar/SideBarStaff/SideBarStaff";
import TableSchedule from "../../../Components/TableSchedule/TableSchedule";

const ScheduleManagement = () => {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);

    const handleSidebarToggle = (isOpen) => {
        setIsSidebarOpen(isOpen);
    };

    return (
        <div className={styles.container}>
            <div className={styles.sidebarFixed}>
                <SideBarStaff onToggle={handleSidebarToggle} />
            </div>
            <div className={`${styles.content} ${isSidebarOpen ? styles.contentOpen : styles.contentClosed}`}>
                <TableSchedule />
            </div>
        </div>
    );
};

export default ScheduleManagement;
