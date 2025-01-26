import React from "react";
import styles from "./BrandLogo.module.css";
import background1 from "./background1.jpg";

export function BrandLogo() {
  return (
    <div className={styles.brandContainer}>
      <img
        src={background1}
        alt="Scenic background"
        className={styles.backgroundImage}
      />
      <div className={styles.brandOverlay}>
        <span className={styles.brandHighlight}>Skin</span>
        <span className={styles.brandText}>Care.</span>
      </div>
    </div>
  );
}

export default BrandLogo;
