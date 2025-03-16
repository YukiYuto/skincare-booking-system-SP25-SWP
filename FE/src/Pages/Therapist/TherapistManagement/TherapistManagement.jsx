import { useState } from "react";
import styles from "./TherapistManagement.module.css";
import SideBarTherapist from "../../../Components/SideBar/SideBarTherapist/SideBarTherapist";
import TableTherapist from "../../../Components/TableTherapist/TableTherapist";

const TherapistManagement = () => {
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
                <TableTherapist />
            </div>
        </div>
    );
};

export default TherapistManagement;
