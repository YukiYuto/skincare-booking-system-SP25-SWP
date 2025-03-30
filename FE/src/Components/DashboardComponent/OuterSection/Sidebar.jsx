import { useState, useEffect } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import styles from "./Sidebar.module.css";
import Brand from "../../Brand/Brand";
import { logout as logoutAction } from "../../../redux/auth/thunks";

import { 
  BarChart3, 
  Users, 
  UserCog, 
  Scissors, 
  ShoppingCart, 
  Calendar
} from 'lucide-react';
import { useDispatch } from 'react-redux';
import { toast } from 'react-toastify';
import { Spin } from 'antd';
import { BookOutlined, HomeOutlined, LogoutOutlined } from '@ant-design/icons';



const Sidebar = ({ onExpandChange }) => {
  const [expanded, setExpanded] = useState(false);

  const handleMouseEnter = () => {
    setExpanded(true);
  };

  const handleMouseLeave = () => {
    setExpanded(false);
  };

  const dispatch = useDispatch();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleLogout = () => {
    setLoading(true);
    try {
      dispatch(logoutAction());
      toast.success("Logout Successfully!");
      navigate("/login");
    } catch (error) {
      toast.error("Logout failed. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  // Notify parent component when expansion state changes
  useEffect(() => {
    if (onExpandChange) {
      onExpandChange(expanded);
    }
  }, [expanded, onExpandChange]);

  const menuItems = [

    { path: '/', label: 'Home', icon: <HomeOutlined size={20} /> },
    { path: '/dashboard/revenue', label: 'Revenue', icon: <BarChart3 size={20} /> },
    { path: '/dashboard/customers', label: 'Customers', icon: <Users size={20} /> },
    { path: '/dashboard/therapists', label: 'Skin Therapists', icon: <UserCog size={20} /> },
    { path: '/dashboard/services', label: 'Services', icon: <Scissors size={20} /> },
    { path: '/dashboard/orders', label: 'Orders', icon: <ShoppingCart size={20} /> },
    { path: '/dashboard/schedule', label: 'Schedule', icon: <Calendar size={20} /> },
    { path: '/dashboard/view-blogcategory', label: 'View Blog', icon: <BookOutlined size={20} /> },

  ];

  return (
    <div
      className={`${styles.sidebar} ${
        expanded ? styles.expanded : styles.collapsed
      }`}
      onMouseEnter={handleMouseEnter}
      onMouseLeave={handleMouseLeave}
    >
      <div className={styles.sidebarHeader}>
        <div className={styles.logoContainer}>
          {expanded ? (
            <h2 className={styles.logo}>
              <Brand />
            </h2>
          ) : (
            <h2 className={styles.logo}>Lc</h2>
          )}
        </div>
      </div>

      <nav className={styles.navLinks}>
        {menuItems.map((item) => (
          <NavLink
            key={item.path}
            to={item.path}
            className={({ isActive }) =>
              `${styles.navItem} ${isActive ? styles.active : ""}`
            }
          >
            <span className={styles.icon}>{item.icon}</span>
            <span className={styles.label}>{item.label}</span>
          </NavLink>
        ))}
        <button
          onClick={handleLogout}
          className={styles.logoutButton}
          disabled={loading}
        >
          <span className={styles.logouticon}>
            <LogoutOutlined />
          </span>
          <span className={styles.label}>
            {loading ? <Spin size="small" /> : "Logout"}
          </span>
        </button>
      </nav>
    </div>
  );
};

export default Sidebar;
