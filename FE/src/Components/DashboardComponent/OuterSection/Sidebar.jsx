import React from 'react';
import { NavLink } from 'react-router-dom';
import styles from './Sidebar.module.css';
import Brand from '../../Brand/Brand';

const Sidebar = () => {
  return (
    <div className={styles.sidebar}>
      <h2 className={styles.logo}><Brand/></h2>
      <nav className={styles.navLinks}>
        <NavLink to="/dashboard/revenue" className={({ isActive }) => isActive ? styles.active : ''}>Revenue</NavLink>
        <NavLink to="/dashboard/customers" className={({ isActive }) => isActive ? styles.active : ''}>Customers</NavLink>
        <NavLink to="/dashboard/therapists" className={({ isActive }) => isActive ? styles.active : ''}>Skin Therapists</NavLink>
        <NavLink to="/dashboard/services" className={({ isActive }) => isActive ? styles.active : ''}>Services</NavLink>
        <NavLink to="/dashboard/orders" className={({ isActive }) => isActive ? styles.active : ''}>Orders</NavLink>
        <NavLink to="/dashboard/schedule" className={({ isActive }) => isActive ? styles.active : ''}>Schedule</NavLink>
      </nav>
    </div>
  );
};

export default Sidebar;