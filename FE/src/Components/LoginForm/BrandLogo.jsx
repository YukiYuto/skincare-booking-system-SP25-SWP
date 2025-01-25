import React from 'react';
import styles from './BrandLogo.module.css';

export function BrandLogo() {
  return (
    <div className={styles.brandContainer}>
      <img
        src="/assets/background.jpg"
        alt="Scenic background"
        className={styles.backgroundImage}
      />
      <div className={styles.brandOverlay}>
        <span className={styles.brandHighlight}>Lanka</span>
        <span className={styles.brandText}>Stay.</span>
      </div>
    </div>
  );
}

export default BrandLogo