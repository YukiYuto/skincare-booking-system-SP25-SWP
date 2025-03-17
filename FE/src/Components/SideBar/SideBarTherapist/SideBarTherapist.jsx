import { useState, useEffect, useCallback } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { logout as logoutAction } from "../../../redux/auth/thunks";
import styles from './SideBarTherapist.module.css';
import { toast } from 'react-toastify';
import { useDispatch } from 'react-redux';
import { Spin } from 'antd';

const SideBarTherapist = ({ onToggle }) => {
    const dispatch = useDispatch();
    const [isOpen, setIsOpen] = useState(true);
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const toggleSidebar = useCallback(() => {
        setIsOpen(prev => !prev);
    }, []);

    const handleLogout = () => {
        setLoading(true);
        try {
            dispatch(logoutAction()); // Chờ logout hoàn thành
            toast.success("Logout Successfully!");
            navigate("/");
        } catch (error) {
            toast.error("Logout failed. Please try again.");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (onToggle) {
            onToggle(isOpen);
        }
    }, [isOpen, onToggle]);

    return (
        <>
            <button onClick={toggleSidebar} className={styles.toggleButton} aria-label="Toggle Sidebar">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor" className={styles.menuIcon}>
                    <path strokeLinecap="round" strokeLinejoin="round" d="M3.75 6.75h16.5M3.75 12h16.5m-16.5 5.25h16.5" />
                </svg>
            </button>
            <aside className={`${styles.sidebar} ${isOpen ? styles.open : styles.closed}`} aria-label="Sidebar">
                <div className={styles.sidebarContent}>
                    <div className={styles.profileSection}>
                        <img className={styles.profileImage} src="https://www.svgrepo.com/show/301043/employee-worker.svg" alt="Profile" />
                        <h2 className={styles.title}>Therapist</h2>
                    </div>
                    <ul className={styles.menuList}>
                        <li>
                            <Link to="/therapist-management" className={styles.menuItem}>
                                <span className={styles.menuText}>Booking</span>
                            </Link>
                        </li>
                        <li>
                            <Link to="/therapist-schedule" className={styles.menuItem}>
                                <span className={styles.menuText}>Schedule</span>
                            </Link>
                        </li>
                    </ul>
                    <button onClick={handleLogout} className={styles.logoutButton} disabled={loading}>
                        {loading ? <Spin size="small" /> : "Logout"}
                    </button>
                </div>
            </aside>
        </>
    );
};

export default SideBarTherapist;
