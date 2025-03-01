import React from 'react';
import styles from './Schedule.module.css';

const Schedule = () => {
  return (
    <div className={styles.tabContainer}>
      <h2 className={styles.tabTitle}>Schedule</h2>
      <p className={styles.tabContent}>This section displays the schedule and appointment details.</p>
    </div>
  );
};

export default Schedule;