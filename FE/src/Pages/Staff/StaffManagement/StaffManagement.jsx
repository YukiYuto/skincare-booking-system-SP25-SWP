import { useState } from "react";
import styles from "./StaffManagement.module.css";
import TableStaff from "../../../Components/TableStaff/TableStaff";
import SideBarStaff from "../../../Components/SideBar/SideBarStaff/SideBarStaff";

const StaffManagement = () => {
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
                <TableStaff />
            </div>
        </div>
    );
};

export default StaffManagement;
