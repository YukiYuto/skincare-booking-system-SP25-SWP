import React from "react";
import styles from "./Revenue.module.css";

const Revenue = () => {
  return (
    <div className={styles.tabContainer}>
      <h2 className={styles.tabTitle}>Revenue</h2>
      <p className={styles.tabContent}>
        This section displays revenue-related data and analytics.
      </p>
    </div>
  );
};

export default Revenue;
