import React from 'react';
import styles from './Customers.module.css';

const Customers = () => {
  return (
    <div className={styles.tabContainer}>
      <h2 className={styles.tabTitle}>Customers</h2>
      <p className={styles.tabContent}>This section displays customer-related data and analytics.</p>
    </div>
  );
};

export default Customers;