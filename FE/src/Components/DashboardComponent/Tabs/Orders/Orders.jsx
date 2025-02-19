import React from 'react';
import styles from './Orders.module.css';

const Orders = () => {
  return (
    <div className={styles.tabContainer}>
      <h2 className={styles.tabTitle}>Services</h2>
      <p className={styles.tabContent}>This section displays available skincare services and details.</p>
    </div>
  );
};

export default Orders;
