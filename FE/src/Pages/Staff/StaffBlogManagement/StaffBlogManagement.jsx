import { useState } from "react";
import styles from "./StaffBlogManagement.module.css";
import SideBarStaff from "../../../Components/SideBar/SideBarStaff/SideBarStaff";
import StaffBlog from "../../../Components/StaffBlog/StaffBlog";

const StaffBlogManagement = () => {
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
                <StaffBlog />
            </div>
        </div>
    );
};

export default StaffBlogManagement;
