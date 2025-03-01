import React from 'react';
import styles from './SkinTherapists.module.css';

const SkinTherapists = () => {
  return (
    <div className={styles.tabContainer}>
      <h2 className={styles.tabTitle}>Skin Therapists</h2>
      <p className={styles.tabContent}>This section displays data and information about skin therapists.</p>
    </div>
  );
};

export default SkinTherapists;
