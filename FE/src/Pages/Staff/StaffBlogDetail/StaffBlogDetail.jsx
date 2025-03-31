import { useState } from "react";
import styles from "./StaffBlogDetail.module.css";
import SideBarStaff from "../../../Components/SideBar/SideBarStaff/SideBarStaff";
import BlogDetail from "../../../Components/StaffBlog/BlogDetail/BlogDetail";

const StaffBlogDetail = () => {
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
                <BlogDetail />
            </div>
        </div>
    );
};

export default StaffBlogDetail;
