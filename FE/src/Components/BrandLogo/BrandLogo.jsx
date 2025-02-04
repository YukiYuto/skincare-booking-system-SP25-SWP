import React from "react";
import styles from "./BrandLogo.module.css";
import background1 from "./background1.jpg";
import Brand from '../Brand/Brand.jsx'

export function BrandLogo() {
  return (
    <div className={styles.brandContainer}>
      <img
        src={background1}
        alt="Scenic background"
        className={styles.backgroundImage}
        loading="lazy"
      />
      <div className={styles.brandOverlay}>
        <Brand/>
      </div>
    </div>
  );
}

export default BrandLogo;