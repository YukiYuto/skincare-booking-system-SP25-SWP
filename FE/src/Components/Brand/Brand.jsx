import React from "react";
import styles from "./Brand.module.css";

const Brand = () => {
  return (
    <div className={styles.brand}>
      <span className={styles.brandHighlight}>Skin</span>
      <span className={styles.brandText}>Topper</span>
    </div>
  );
};

export default Brand;
